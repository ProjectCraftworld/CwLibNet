using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.Extensions;
using CwLibNet.IO;
using CwLibNet.IO.Serializer;
using CwLibNet.IO.Streams;
using CwLibNet.Structs.Animation;
using CwLibNet.Types;

namespace CwLibNet.Resources;

public class RAnimation: Resource
{
    public enum AnimationType
    {
        ROTATION,
        SCALE,
        POSITION
    }

    public class RpsAnimData
    {
        public Vector4?[] Rot;
        public Vector4?[] Pos;
        public Vector4?[] Scale;
        public float[] Morph;

        public RpsAnimData(RAnimation animation)
        {
            Rot = new Vector4?[animation.Bones.Length];
            Pos = new Vector4?[animation.Bones.Length];
            Scale = new Vector4?[animation.Bones.Length];
            Morph = new float[animation.MorphCount];
        }
    }

    public const int BaseAllocationSize = 0x100;

    public AnimBone[] Bones;

    public short NumFrames, Fps = 24, LoopStart;
    public byte MorphCount;

    public byte[] RotBonesAnimated;
    public byte[] PosBonesAnimated;
    public byte[] ScaledBonesAnimated;
    public byte[] MorphsAnimated;

    public Vector4 PosOffset = new Vector4(0.0f, 0.0f, 0.0f, 1.0f), PosScale =
        new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
    public bool Fat; // if fat, each element is 4 bytes, otherwise 2 bytes, probably just
    // alignment?

    public Vector4?[] PackedRotation;
    public Vector4?[] PackedPosition;
    public Vector4?[] PackedScale;
    public float[] PackedMorph;

    public Locator[] Locators;

    private RpsAnimData[] cachedFrameData;

    public static int CalculateAnimationHash(String value)
    {
        if (value == null) return 0;
        long animHash = 0, offset = 0;

        if (value.Contains(" R "))
        {
            offset = 3;
            value = value.Replace(" R ", "");
        }
        else if (value.Contains(" L "))
        {
            offset = 2;
            value = value.Replace(" L ", "");
        }

        animHash = value.Aggregate(animHash, (current, c) => ((long)c) + current * 0x1003fL);

        return (int) (((animHash * 4L) & 0xFFFFFFFFL) + offset);
    }

    
    public override void Serialize(Serializer serializer)
    {
        int version = serializer.GetRevision().GetVersion();
        bool isWriting = serializer.IsWriting();

        if (version < 0x378)
        {
            Bones = serializer.Array(Bones);

            NumFrames = (short) serializer.I32(NumFrames);
            Fps = (short) serializer.I32(Fps);
            MorphCount = (byte) serializer.I32(MorphCount);

            MemoryInputStream input = serializer.GetInput();
            MemoryOutputStream output = serializer.GetOutput();

            // Should I just throw these into some other function?
            // Pretty repetitive

            if (!isWriting) RotBonesAnimated = new byte[input.I32()];
            else output.I32(RotBonesAnimated.Length);
            for (int i = 0; i < RotBonesAnimated.Length; ++i)
                RotBonesAnimated[i] = (byte) serializer.I16(RotBonesAnimated[i]);

            if (!isWriting) PosBonesAnimated = new byte[input.I32()];
            else output.I32(PosBonesAnimated.Length);
            for (int i = 0; i < PosBonesAnimated.Length; ++i)
                PosBonesAnimated[i] = (byte) serializer.I16(PosBonesAnimated[i]);

            if (!isWriting) ScaledBonesAnimated = new byte[input.I32()];
            else output.I32(ScaledBonesAnimated.Length);
            for (int i = 0; i < ScaledBonesAnimated.Length; ++i)
                ScaledBonesAnimated[i] = (byte) serializer.I16(ScaledBonesAnimated[i]);

            if (!isWriting) MorphsAnimated = new byte[input.I32()];
            else output.I32(MorphsAnimated.Length);
            for (int i = 0; i < MorphsAnimated.Length; ++i)
                MorphsAnimated[i] = (byte) serializer.I16(MorphsAnimated[i]);

            if (!isWriting) PackedRotation = new Vector4?[input.I32()];
            else output.I32(PackedRotation.Length);
            for (int i = 0; i < PackedRotation.Length; ++i)
                PackedRotation[i] = serializer.V4(PackedRotation[i]);

            if (!isWriting) PackedPosition = new Vector4?[input.I32()];
            else output.I32(PackedPosition.Length);
            for (int i = 0; i < PackedPosition.Length; ++i)
                PackedPosition[i] = serializer.V4(PackedPosition[i]);

            if (!isWriting) PackedScale = new Vector4?[input.I32()];
            else output.I32(PackedScale.Length);
            for (int i = 0; i < PackedScale.Length; ++i)
                PackedScale[i] = serializer.V4(PackedScale[i]);

            if (!isWriting) PackedMorph = new float[input.I32()];
            else output.I32(PackedMorph.Length);
            for (int i = 0; i < PackedMorph.Length; ++i)
                PackedMorph[i] = serializer.F32(PackedMorph[i]);

            // TODO: Locators when revision > 0x311

            return;
        }

        NumFrames = serializer.I16(NumFrames);
        Fps = serializer.I16(Fps);
        LoopStart = serializer.I16(LoopStart);
        MorphCount = serializer.I8(MorphCount);

        if (isWriting)
        {
            MemoryOutputStream memoryOutputStream = new MemoryOutputStream(0x50000);
            foreach (AnimBone bone in Bones)
            {
                memoryOutputStream.I32(bone.animHash);
                memoryOutputStream.U8(bone.parent);
                memoryOutputStream.U8(bone.firstChild);
                memoryOutputStream.U8(bone.nextSibling);
                memoryOutputStream.U8(0); // Padding
            }

            foreach (byte bone in RotBonesAnimated) memoryOutputStream.I8(bone);
            foreach (byte bone in PosBonesAnimated) memoryOutputStream.I8(bone);
            foreach (byte bone in ScaledBonesAnimated) memoryOutputStream.I8(bone);
            foreach (byte bone in MorphsAnimated) memoryOutputStream.I8(bone);

            if ((memoryOutputStream.GetOffset() % 2) != 0) memoryOutputStream.U8(0);

            foreach (Vector4 rotation in PackedRotation)
            {
                short xrot = (short) Math.Round(rotation.X * 0x7fff);
                xrot &= ~1;
                if (rotation.W < 0.0f) xrot |= 1;

                memoryOutputStream.I16(xrot);
                memoryOutputStream.I16((short) (Math.Round(rotation.Y * 0x7fff)));
                memoryOutputStream.I16((short) (Math.Round(rotation.Z * 0x7fff)));
            }

            foreach (Vector4 position in PackedPosition)
            {
                memoryOutputStream.F16(position.X);
                memoryOutputStream.F16(position.Y);
                memoryOutputStream.F16(position.Z);
            }

            foreach (Vector4 scale in PackedScale)
            {
                memoryOutputStream.F16(scale.X);
                memoryOutputStream.F16(scale.Y);
                memoryOutputStream.F16(scale.Z);
            }

            foreach (float morph in PackedMorph) memoryOutputStream.F16(morph);

            while (memoryOutputStream.GetOffset() % 16 != 0) memoryOutputStream.U8(0);

            memoryOutputStream.Shrink();

            byte[]? animationData = memoryOutputStream.GetBuffer();
            memoryOutputStream = serializer.GetOutput();

            memoryOutputStream.U8(Bones.Length);
            memoryOutputStream.U8(RotBonesAnimated.Length);
            memoryOutputStream.U8(PosBonesAnimated.Length);
            memoryOutputStream.U8(ScaledBonesAnimated.Length);
            memoryOutputStream.U8(MorphsAnimated.Length);
            memoryOutputStream.U16(0); // locatorKeys

            if (version > 0x38b)
            {
                memoryOutputStream.V4(PosOffset);
                memoryOutputStream.V4(PosScale);
                if (version > 0x3b1)
                    memoryOutputStream.Boole(Fat);
            }

            memoryOutputStream.Bytearray(animationData);

            serializer.Array(Locators);

            return;
        }

        MemoryInputStream stream = serializer.GetInput();

        int boneCount = stream.U8();
        int rotAnims = stream.U8();
        int posAnims = stream.U8();
        int scaleAnims = stream.U8();
        int morphAnims = stream.U8();
        int locatorKeys = stream.U16();

        if (version > 0x38b)
        {
            PosOffset = stream.V4();
            PosScale = stream.V4();
            if (version > 0x3b1)
                Fat = stream.Boole();
        }

        byte[] animData = stream.Bytearray();

        Locators = serializer.Array(Locators);

        stream = new MemoryInputStream(animData);

        Bones = new AnimBone[boneCount];
        for (int i = 0; i < boneCount; ++i)
        {
            Bones[i] = new AnimBone(stream.I32(), stream.I8(), stream.I8(), stream.I8());
            stream.I8();
        }

        RotBonesAnimated = new byte[rotAnims];
        PosBonesAnimated = new byte[posAnims];
        ScaledBonesAnimated = new byte[scaleAnims];
        MorphsAnimated = new byte[morphAnims];

        if (locatorKeys != 0) stream.Bytes(0x4 * locatorKeys);

        for (int i = 0; i < rotAnims; ++i) RotBonesAnimated[i] = stream.I8();
        for (int i = 0; i < posAnims; ++i) PosBonesAnimated[i] = stream.I8();
        for (int i = 0; i < scaleAnims; ++i) ScaledBonesAnimated[i] = stream.I8();
        for (int i = 0; i < morphAnims; ++i) MorphsAnimated[i] = stream.I8();

        if (stream.GetOffset() % 2 != 0) stream.I8(); // Alignment

        PackedRotation = new Vector4?[boneCount + (rotAnims * (NumFrames - 1))];
        PackedPosition = new Vector4?[boneCount + (posAnims * (NumFrames - 1))];
        PackedScale = new Vector4?[boneCount + (scaleAnims * (NumFrames - 1))];
        PackedMorph = new float[MorphCount + (morphAnims * (NumFrames - 1))];

        for (int i = 0; i < PackedRotation.Length; ++i)
        {
            short xrot = stream.I16();
            bool flag = (xrot & 1) != 0;
            xrot &= (short) ~1;

            float x = (float) xrot / 0x7FFF;
            float y = ((float) stream.I16()) / 0x7FFF;
            float z = ((float) stream.I16()) / 0x7FFF;
            float w =
                (float) Math.Sqrt(1 - ((Math.Pow(x, 2)) + (Math.Pow(y, 2) + (Math.Pow(z
                    , 2)))));

            PackedRotation[i] = new Vector4(x, y, z, flag ? -w : w);
        }

        for (int i = 0; i < PackedPosition.Length; ++i)
            PackedPosition[i] = new Vector4(stream.F16(), stream.F16(), stream.F16(), 1.0f);

        for (int i = 0; i < PackedScale.Length; ++i)
            PackedScale[i] = new Vector4(stream.F16(), stream.F16(), stream.F16(), 1.0f);

        for (int i = 0; i < PackedMorph.Length; ++i)
            PackedMorph[i] = stream.F16();

    }

    public int GetLoopedFrame(int frame, bool looped)
    {
        if (looped)
        {
            int nf = (frame - LoopStart) % (NumFrames - LoopStart);
            if (nf < 0)
                nf += (NumFrames - LoopStart);
            return LoopStart + nf;
        }

        if (frame < 0) return 0;
        if (NumFrames <= frame)
            return NumFrames - 1;
        return frame;
    }

    private void Unpack(Vector4?[] @out, Vector4?[] src, int animatedSize, int frame,
                        int boneCount, byte[] animated)
    {
        if (animatedSize == 0) return;
        int offset = animatedSize * (frame - 1) + boneCount;
        for (int bone = 0; bone < animatedSize; ++bone)
            @out[animated[bone] & 0xff] = src[offset + bone];
    }

    private RpsAnimData GetAnimDataForFrame(int frame)
    {
        RpsAnimData data = new RpsAnimData(this);

        for (int i = 0; i < Bones.Length; ++i)
        {
            data.Rot[i] = PackedRotation[i];
            data.Pos[i] = PackedPosition[i];
            data.Scale[i] = PackedScale[i];
        }

        if (MorphCount >= 0)
            Array.Copy(PackedMorph, 0, data.Morph, 0, MorphCount);

        if (frame > 0)
        {
            Unpack(data.Rot, PackedRotation, RotBonesAnimated.Length, frame,
                Bones.Length, RotBonesAnimated);
            Unpack(data.Pos, PackedPosition, PosBonesAnimated.Length, frame,
                Bones.Length, PosBonesAnimated);
            Unpack(data.Scale, PackedScale, ScaledBonesAnimated.Length, frame,
                Bones.Length, ScaledBonesAnimated);
        }

        int offset = MorphsAnimated.Length * (frame - 1) + MorphCount;
        for (int morph = 0; morph < MorphsAnimated.Length; ++morph)
            data.Morph[MorphsAnimated[morph] & 0xff] = PackedMorph[offset + morph];

        return data;
    }

    public Vector4? GetBasePosition(int boneIndex)
    {
        return PackedPosition[boneIndex];
    }

    public Vector4? GetBaseRotation(int boneIndex)
    {
        return PackedRotation[boneIndex];
    }

    public Vector4? GetBaseScale(int boneIndex)
    {
        return PackedScale[boneIndex];
    }

    public float GetBaseWeight(int morphIndex)
    {
        return PackedMorph[morphIndex];
    }

    public Matrix4x4 GetBaseTransform(int animHash)
    {
        int index = GetBoneIndex(animHash);
        if (index == -1) return Matrix4x4.Identity;
        Vector4 pos = PackedPosition[index].Value;
        Vector4 rot = PackedRotation[index].Value;
        Vector4 sx = PackedScale[index].Value;

        Vector3? translation = new Vector3(pos.X, pos.Y, pos.Z);
        Quaternion quaternion = new Quaternion(rot.X, rot.Y, rot.Z, rot.W);
        Vector3 scale = new Vector3(sx.X, sx.Y, sx.Y);

        return Matrix4x4.Identity.TranslationRotationScale(
            translation.Value,
            quaternion,
            scale
        );
    }

    private void Cache()
    {
        if (cachedFrameData == null)
        {
            cachedFrameData = new RpsAnimData[NumFrames];
            for (int i = 0; i < NumFrames; ++i)
                cachedFrameData[i] = GetAnimDataForFrame(i);
        }
    }

    public float[] GetFrameWeights(int frame, float position, bool looped)
    {
        Cache();
        return cachedFrameData[GetLoopedFrame(frame, looped)].Morph;
    }

    public float[] GetBlendedFrameWeights(int frame, float position, bool looped)
    {
        Cache();

        float lastTime = frame * (1.0f / NumFrames);
        float nextTime = (frame + 1) * (1.0f / NumFrames);
        float scaleFactor = (position - lastTime) / (nextTime - lastTime);

        RpsAnimData data = cachedFrameData[GetLoopedFrame(frame, looped)];
        RpsAnimData next = cachedFrameData[GetLoopedFrame(frame + 1, looped)];

        float[] weights = new float[data.Morph.Length];
        for (int i = 0; i < weights.Length; i++)
            weights[i] = data.Morph[i] + scaleFactor * (next.Morph[i] - data.Morph[i]);

        return weights;
    }

    public RpsAnimData GetFrameData(int frame, float position, bool looped)
    {
        Cache();
        return cachedFrameData[GetLoopedFrame(frame, looped)];
    }

    public Matrix4x4? GetBlendedFrameMatrix(int animHash, int frame, float position, bool looped)
    {
        float lastTime = frame * (1.0f / NumFrames);
        float nextTime = (frame + 1) * (1.0f / NumFrames);
        float scaleFactor = (position - lastTime) / (nextTime - lastTime);

        Cache();

        int index = GetBoneIndex(animHash);
        if (index == -1) return null;

        RpsAnimData data = cachedFrameData[GetLoopedFrame(frame, looped)];
        RpsAnimData next = cachedFrameData[GetLoopedFrame(frame + 1, looped)];

        Vector4 pos = data.Pos[index].Value.Lerp(next.Pos[index].Value, scaleFactor);
        Vector4 scale = data.Scale[index].Value.Lerp(next.Scale[index].Value, scaleFactor);
        Quaternion rot = Quaternion.Slerp(new Quaternion(
            data.Rot[index].Value.X, data.Rot[index].Value.Y, data.Rot[index].Value.Z, data.Rot[index].Value.W
        ),
            new Quaternion(
                next.Rot[index].Value.X, next.Rot[index].Value.Y, next.Rot[index].Value.Z,
                next.Rot[index].Value.W
            ),
            scaleFactor
        );

        return Matrix4x4.Identity.TranslationRotationScale(
            new Vector3(pos.X, pos.Y, pos.Z), rot, new Vector3(scale.X, scale.Y, scale.Z)
        );
    }

    public Matrix4x4? GetFrameMatrix(int animHash, int frame, float position, bool looped)
    {
        Cache();

        int index = GetBoneIndex(animHash);
        if (index == -1) return null;


        RpsAnimData data = cachedFrameData[GetLoopedFrame(frame, looped)];

        Vector3 scale = new Vector3(data.Scale[index].Value.X, data.Scale[index].Value.Y,
            data.Scale[index].Value.Z);

        return Matrix4x4.Identity.TranslationRotationScale(
            new Vector3(data.Pos[index].Value.X, data.Pos[index].Value.Y, data.Pos[index].Value.Z),
            new Quaternion(data.Rot[index].Value.X, data.Rot[index].Value.Y, data.Rot[index].Value.Z,
                data.Rot[index].Value.W),
            scale
        );
    }

    public Matrix4x4 GetFrameMatrix(int frame, int index)
    {
        Cache();
        RpsAnimData data = cachedFrameData[frame];
        Vector3 scale = new Vector3(data.Scale[index].Value.X, data.Scale[index].Value.Y,
            data.Scale[index].Value.Z);
        return Matrix4x4.Identity.TranslationRotationScale(
            new Vector3(data.Pos[index].Value.X, data.Pos[index].Value.Y, data.Pos[index].Value.Z),
            new Quaternion(data.Rot[index].Value.X, data.Rot[index].Value.Y, data.Rot[index].Value.Z,
                data.Rot[index].Value.W),
            scale
        );
    }

    public Matrix4x4 GetWorldPosition(int frame, int index)
    {
        Cache();
        List<Matrix4x4> sequence = [];
        sequence.Add(GetFrameMatrix(frame, index));
        index = Bones[index].parent;
        while (index != -1)
        {
            sequence.Add(GetFrameMatrix(frame, index));
            index = Bones[index].parent;
        }
        Matrix4x4 wpos = new Matrix4x4();
        for (int i = sequence.Count - 1; i >= 0; i--)
            wpos *= sequence[i];
        return wpos;
    }

    public void ToZUp()
    {
        Cache();

        Quaternion f = GetFrameMatrix(0, 2).GetUnnormalizedRotation();
        Matrix4x4[] matrices = new Matrix4x4[Bones.Length];
        for (int i = 0; i < Bones.Length; i++)
            matrices[i] = GetWorldPosition(0, i);
        // matrices[0].rotateX((float) Math.toRadians(90.0f));
        // matrices[0].mapXZY();

        for (int i = 0; i < Bones.Length; i++)
        {
            Matrix4x4 frame = matrices[i];
            if (Bones[i].parent != -1)
            {
                Matrix4x4 parent = matrices[Bones[i].parent];
                frame = parent.Invert() * frame;
            }

            Matrix4x4 myRot =
                GetFrameMatrix(0, i).GetUnnormalizedRotation().GetMatrix();
            if (Bones[i].parent != -1)
            {
                Matrix4x4 p =
                    GetFrameMatrix(0, Bones[i].parent).GetUnnormalizedRotation().GetMatrix();
                myRot = p.Invert() * (myRot);
            }

            Quaternion rot = myRot.GetNormalizedRotation();
            rot.RotateLocalX((90.0f).ToRadians());

            Vector3 pos = frame.GetTranslation();

            PackedPosition[i] = new Vector4(pos, 1.0f);
            PackedRotation[i] = new Vector4(rot.X, rot.Y, rot.Z, rot.W);
            PackedScale[i] = new Vector4(frame.GetScale(), 1.0f);
        }

        for (int i = 1; i < NumFrames; i++)
        {
            for (int b = 0; b < Bones.Length; b++)
                matrices[b] = GetWorldPosition(i, b);


        }


    }

    public float[] GetBaseWeights()
    {
        float[] weights = new float[MorphCount];
        Array.Copy(PackedMorph, 0, weights, 0, MorphCount);
        return weights;
    }

    public int GetBoneIndex(int animHash)
    {
        if (animHash == 0) return 0;
        for (int i = 0; i < Bones.Length; ++i)
        {
            AnimBone bone = Bones[i];
            if (bone.animHash == animHash)
                return i;
        }
        return -1;
    }

    public bool IsAnimated(int morph)
    {
        return MorphsAnimated.Any(index => index == morph);
    }

    public bool IsAnimatedAtAll(int animHash)
    {
        byte index = (byte) GetBoneIndex(animHash);
        if (PosBonesAnimated.Any(animated => animated == index))
        {
            return true;
        }
        return RotBonesAnimated.Any(animated => animated == index) || ScaledBonesAnimated.Any(animated => animated == index);
    }

    public bool IsAnimated(AnimBone bone, AnimationType type)
    {
        if (bone == null) return false;
        return IsAnimated(bone.animHash, type);
    }

    public bool IsAnimated(int animHash, AnimationType type)
    {
        return GetAnimationIndex(animHash, type) != -1;
    }

    public int GetAnimationIndex(int animHash, AnimationType type)
    {
        byte[] indices = null;
        switch (type)
        {
            case AnimationType.ROTATION:
                indices = RotBonesAnimated;
                break;
            case AnimationType.POSITION:
                indices = PosBonesAnimated;
                break;
            case AnimationType.SCALE:
                indices = ScaledBonesAnimated;
                break;
        }

        if (indices == null) return -1;
        int boneIndex = GetBoneIndex(animHash);
        if (boneIndex == -1) return -1;

        for (int i = 0; i < indices.Length; ++i)
        {
            int animBoneIndex = indices[i] & 0xff;
            if (animBoneIndex == boneIndex)
                return i;
        }

        return -1;
    }

    public Vector3 GetTranslationFrame(int animHash, int frame)
    {
        int index = GetBoneIndex(animHash);
        if (index == -1) return new Vector3();
        Vector4 translation;
        int animIndex = GetAnimationIndex(animHash, AnimationType.POSITION);
        if (frame == 0 || animIndex == -1) translation = PackedPosition[index].Value;
        else
            translation =
                PackedPosition[Bones.Length + ((frame - 1) * PosBonesAnimated.Length) + animIndex].Value;

        return new Vector3(translation.X, translation.Y, translation.Z);
    }

    public Quaternion GetRotationFrame(int animHash, int frame)
    {
        int index = GetBoneIndex(animHash);
        if (index == -1) return new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        Vector4 rotation;
        int animIndex = GetAnimationIndex(animHash, AnimationType.ROTATION);
        if (frame == 0 || animIndex == -1)
            rotation = PackedRotation[index].Value;
        else
            rotation =
                PackedRotation[Bones.Length + ((frame - 1) * RotBonesAnimated.Length) + animIndex].Value;

        return new Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W);
    }

    public Vector3 GetScaleFrame(int animHash, int frame)
    {
        int index = GetBoneIndex(animHash);
        if (index == -1) return new Vector3(1.0f, 1.0f, 1.0f);
        Vector4 scale;
        int animIndex = GetAnimationIndex(animHash, AnimationType.SCALE);
        if (frame == 0 || animIndex == -1)
            scale = PackedScale[index].Value;
        else
            scale =
                PackedScale[Bones.Length + ((frame - 1) * ScaledBonesAnimated.Length) + animIndex].Value;

        return new Vector3(scale.X, scale.Y, scale.Z);
    }

    private Vector4[] GetFrames(int animHash, AnimationType type)
    {
        int animIndex = GetAnimationIndex(animHash, type);
        if (animIndex == -1) return null;

        int boneIndex = GetBoneIndex(animHash);
        if (boneIndex == -1) return null;

        Vector4?[] pack = null;
        int animated = 0;
        switch (type)
        {
            case AnimationType.ROTATION:
            {
                pack = PackedRotation;
                animated = RotBonesAnimated.Length;
                break;
            }
            case AnimationType.POSITION:
            {
                pack = PackedPosition;
                animated = PosBonesAnimated.Length;
                break;
            }
            case AnimationType.SCALE:
            {
                pack = PackedScale;
                animated = ScaledBonesAnimated.Length;
                break;
            }
            default:
                return null;
        }

        Vector4[] frames = new Vector4[NumFrames];
        frames[0] = pack[boneIndex].Value;
        for (int i = 1; i < NumFrames; ++i)
            frames[i] = pack[Bones.Length + ((i - 1) * animated) + animIndex].Value;

        return frames;
    }

    public Vector4[] GetRotationFrames(AnimBone bone)
    {
        return GetRotationFrames(bone.animHash);
    }

    public Vector4[] GetRotationFrames(int animHash)
    {
        return GetFrames(animHash, AnimationType.ROTATION);
    }

    public Vector4[] GetPositionFrames(AnimBone bone)
    {
        return GetPositionFrames(bone.animHash);
    }

    public Vector4[] GetPositionFrames(int animHash)
    {
        return GetFrames(animHash, AnimationType.POSITION);
    }

    public Vector4[] GetScaleFrames(AnimBone bone)
    {
        return GetScaleFrames(bone.animHash);
    }

    public Vector4[] GetScaleFrames(int animHash)
    {
        return GetFrames(animHash, AnimationType.SCALE);
    }

    public float[] GetMorphFrames(int index)
    {
        if (index < 0 || index >= MorphCount || !IsAnimated(index)) return null;
        float[] frames = new float[NumFrames];
        frames[0] = PackedMorph[index];
        for (int i = 1; i < NumFrames; ++i)
            frames[i] =
                PackedMorph[MorphCount + ((i - 1) * MorphsAnimated.Length) + index];
        return frames;

    }

    
    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        Serializer serializer = new Serializer(GetAllocatedSize(), revision,
            compressionFlags);
        serializer.Struct(this);
        return new SerializationData(
            serializer.GetBuffer(),
            revision,
            compressionFlags,
            ResourceType.Animation,
            SerializationType.BINARY,
            serializer.GetDependencies()
        );
    }

    
    public override int GetAllocatedSize()
    {
        int size = BaseAllocationSize;
        if (Bones != null) size += (Bones.Length * AnimBone.BASE_ALLOCATION_SIZE);
        if (RotBonesAnimated != null) size += (RotBonesAnimated.Length * 2);
        if (PosBonesAnimated != null) size += (PosBonesAnimated.Length * 2);
        if (ScaledBonesAnimated != null) size += (ScaledBonesAnimated.Length * 2);
        if (MorphsAnimated != null) size += (MorphsAnimated.Length * 2);
        if (PackedRotation != null) size += (PackedRotation.Length * 0x10);
        if (PackedPosition != null) size += (PackedPosition.Length * 0x10);
        if (PackedScale != null) size += (PackedScale.Length * 0x10);
        if (PackedMorph != null) size += (PackedMorph.Length * 0x4);
        if (Locators != null) size += Locators.Sum(locator => (locator.GetAllocatedSize()));
        return size;
    }


}