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
            this.Rot = new Vector4?[animation.Bones.Length];
            this.Pos = new Vector4?[animation.Bones.Length];
            this.Scale = new Vector4?[animation.Bones.Length];
            this.Morph = new float[animation.MorphCount];
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
            foreach (AnimBone bone in this.Bones)
            {
                memoryOutputStream.I32(bone.animHash);
                memoryOutputStream.U8(bone.parent);
                memoryOutputStream.U8(bone.firstChild);
                memoryOutputStream.U8(bone.nextSibling);
                memoryOutputStream.U8(0); // Padding
            }

            foreach (byte bone in this.RotBonesAnimated) memoryOutputStream.I8(bone);
            foreach (byte bone in this.PosBonesAnimated) memoryOutputStream.I8(bone);
            foreach (byte bone in this.ScaledBonesAnimated) memoryOutputStream.I8(bone);
            foreach (byte bone in this.MorphsAnimated) memoryOutputStream.I8(bone);

            if ((memoryOutputStream.GetOffset() % 2) != 0) memoryOutputStream.U8(0);

            foreach (Vector4 rotation in this.PackedRotation)
            {
                short xrot = (short) Math.Round(rotation.X * 0x7fff);
                xrot &= ~1;
                if (rotation.W < 0.0f) xrot |= 1;

                memoryOutputStream.I16(xrot);
                memoryOutputStream.I16((short) (Math.Round(rotation.Y * 0x7fff)));
                memoryOutputStream.I16((short) (Math.Round(rotation.Z * 0x7fff)));
            }

            foreach (Vector4 position in this.PackedPosition)
            {
                memoryOutputStream.F16(position.X);
                memoryOutputStream.F16(position.Y);
                memoryOutputStream.F16(position.Z);
            }

            foreach (Vector4 scale in this.PackedScale)
            {
                memoryOutputStream.F16(scale.X);
                memoryOutputStream.F16(scale.Y);
                memoryOutputStream.F16(scale.Z);
            }

            foreach (float morph in this.PackedMorph) memoryOutputStream.F16(morph);

            while (memoryOutputStream.GetOffset() % 16 != 0) memoryOutputStream.U8(0);

            memoryOutputStream.Shrink();

            byte[]? animationData = memoryOutputStream.GetBuffer();
            memoryOutputStream = serializer.GetOutput();

            memoryOutputStream.U8(this.Bones.Length);
            memoryOutputStream.U8(this.RotBonesAnimated.Length);
            memoryOutputStream.U8(this.PosBonesAnimated.Length);
            memoryOutputStream.U8(this.ScaledBonesAnimated.Length);
            memoryOutputStream.U8(this.MorphsAnimated.Length);
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
            int nf = (frame - this.LoopStart) % (this.NumFrames - this.LoopStart);
            if (nf < 0)
                nf += (this.NumFrames - this.LoopStart);
            return this.LoopStart + nf;
        }

        if (frame < 0) return 0;
        if (this.NumFrames <= frame)
            return this.NumFrames - 1;
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

        for (int i = 0; i < this.Bones.Length; ++i)
        {
            data.Rot[i] = this.PackedRotation[i];
            data.Pos[i] = this.PackedPosition[i];
            data.Scale[i] = this.PackedScale[i];
        }

        if (this.MorphCount >= 0)
            Array.Copy(this.PackedMorph, 0, data.Morph, 0, this.MorphCount);

        if (frame > 0)
        {
            this.Unpack(data.Rot, this.PackedRotation, this.RotBonesAnimated.Length, frame,
                this.Bones.Length, this.RotBonesAnimated);
            this.Unpack(data.Pos, this.PackedPosition, this.PosBonesAnimated.Length, frame,
                this.Bones.Length, this.PosBonesAnimated);
            this.Unpack(data.Scale, this.PackedScale, this.ScaledBonesAnimated.Length, frame,
                this.Bones.Length, this.ScaledBonesAnimated);
        }

        int offset = this.MorphsAnimated.Length * (frame - 1) + this.MorphCount;
        for (int morph = 0; morph < this.MorphsAnimated.Length; ++morph)
            data.Morph[this.MorphsAnimated[morph] & 0xff] = this.PackedMorph[offset + morph];

        return data;
    }

    public Vector4? GetBasePosition(int boneIndex)
    {
        return this.PackedPosition[boneIndex];
    }

    public Vector4? GetBaseRotation(int boneIndex)
    {
        return this.PackedRotation[boneIndex];
    }

    public Vector4? GetBaseScale(int boneIndex)
    {
        return this.PackedScale[boneIndex];
    }

    public float GetBaseWeight(int morphIndex)
    {
        return this.PackedMorph[morphIndex];
    }

    public Matrix4x4 GetBaseTransform(int animHash)
    {
        int index = this.GetBoneIndex(animHash);
        if (index == -1) return Matrix4x4.Identity;
        Vector4 pos = this.PackedPosition[index].Value;
        Vector4 rot = this.PackedRotation[index].Value;
        Vector4 sx = this.PackedScale[index].Value;

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
        if (this.cachedFrameData == null)
        {
            this.cachedFrameData = new RpsAnimData[this.NumFrames];
            for (int i = 0; i < this.NumFrames; ++i)
                this.cachedFrameData[i] = this.GetAnimDataForFrame(i);
        }
    }

    public float[] GetFrameWeights(int frame, float position, bool looped)
    {
        this.Cache();
        return this.cachedFrameData[this.GetLoopedFrame(frame, looped)].Morph;
    }

    public float[] GetBlendedFrameWeights(int frame, float position, bool looped)
    {
        this.Cache();

        float lastTime = frame * (1.0f / this.NumFrames);
        float nextTime = (frame + 1) * (1.0f / this.NumFrames);
        float scaleFactor = (position - lastTime) / (nextTime - lastTime);

        RpsAnimData data = this.cachedFrameData[this.GetLoopedFrame(frame, looped)];
        RpsAnimData next = this.cachedFrameData[this.GetLoopedFrame(frame + 1, looped)];

        float[] weights = new float[data.Morph.Length];
        for (int i = 0; i < weights.Length; i++)
            weights[i] = data.Morph[i] + scaleFactor * (next.Morph[i] - data.Morph[i]);

        return weights;
    }

    public RpsAnimData GetFrameData(int frame, float position, bool looped)
    {
        this.Cache();
        return this.cachedFrameData[this.GetLoopedFrame(frame, looped)];
    }

    public Matrix4x4? GetBlendedFrameMatrix(int animHash, int frame, float position, bool looped)
    {
        float lastTime = frame * (1.0f / this.NumFrames);
        float nextTime = (frame + 1) * (1.0f / this.NumFrames);
        float scaleFactor = (position - lastTime) / (nextTime - lastTime);

        this.Cache();

        int index = this.GetBoneIndex(animHash);
        if (index == -1) return null;

        RpsAnimData data = this.cachedFrameData[this.GetLoopedFrame(frame, looped)];
        RpsAnimData next = this.cachedFrameData[this.GetLoopedFrame(frame + 1, looped)];

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
        this.Cache();

        int index = this.GetBoneIndex(animHash);
        if (index == -1) return null;


        RpsAnimData data = this.cachedFrameData[this.GetLoopedFrame(frame, looped)];

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
        this.Cache();
        RpsAnimData data = this.cachedFrameData[frame];
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
        this.Cache();
        List<Matrix4x4> sequence = [];
        sequence.Add(this.GetFrameMatrix(frame, index));
        index = this.Bones[index].parent;
        while (index != -1)
        {
            sequence.Add(this.GetFrameMatrix(frame, index));
            index = this.Bones[index].parent;
        }
        Matrix4x4 wpos = new Matrix4x4();
        for (int i = sequence.Count - 1; i >= 0; i--)
            wpos *= sequence[i];
        return wpos;
    }

    public void ToZUp()
    {
        this.Cache();

        Quaternion f = this.GetFrameMatrix(0, 2).GetUnnormalizedRotation();
        Matrix4x4[] matrices = new Matrix4x4[this.Bones.Length];
        for (int i = 0; i < this.Bones.Length; i++)
            matrices[i] = this.GetWorldPosition(0, i);
        // matrices[0].rotateX((float) Math.toRadians(90.0f));
        // matrices[0].mapXZY();

        for (int i = 0; i < this.Bones.Length; i++)
        {
            Matrix4x4 frame = matrices[i];
            if (this.Bones[i].parent != -1)
            {
                Matrix4x4 parent = matrices[this.Bones[i].parent];
                frame = parent.Invert() * frame;
            }

            Matrix4x4 myRot =
                this.GetFrameMatrix(0, i).GetUnnormalizedRotation().GetMatrix();
            if (this.Bones[i].parent != -1)
            {
                Matrix4x4 p =
                    this.GetFrameMatrix(0, this.Bones[i].parent).GetUnnormalizedRotation().GetMatrix();
                myRot = p.Invert() * (myRot);
            }

            Quaternion rot = myRot.GetNormalizedRotation();
            rot.RotateLocalX((90.0f).ToRadians());

            Vector3 pos = frame.GetTranslation();

            this.PackedPosition[i] = new Vector4(pos, 1.0f);
            this.PackedRotation[i] = new Vector4(rot.X, rot.Y, rot.Z, rot.W);
            this.PackedScale[i] = new Vector4(frame.GetScale(), 1.0f);
        }

        for (int i = 1; i < this.NumFrames; i++)
        {
            for (int b = 0; b < this.Bones.Length; b++)
                matrices[b] = this.GetWorldPosition(i, b);


        }


    }

    public float[] GetBaseWeights()
    {
        float[] weights = new float[this.MorphCount];
        Array.Copy(this.PackedMorph, 0, weights, 0, this.MorphCount);
        return weights;
    }

    public int GetBoneIndex(int animHash)
    {
        if (animHash == 0) return 0;
        for (int i = 0; i < this.Bones.Length; ++i)
        {
            AnimBone bone = this.Bones[i];
            if (bone.animHash == animHash)
                return i;
        }
        return -1;
    }

    public bool IsAnimated(int morph)
    {
        return this.MorphsAnimated.Any(index => index == morph);
    }

    public bool IsAnimatedAtAll(int animHash)
    {
        byte index = (byte) this.GetBoneIndex(animHash);
        if (this.PosBonesAnimated.Any(animated => animated == index))
        {
            return true;
        }
        return this.RotBonesAnimated.Any(animated => animated == index) || this.ScaledBonesAnimated.Any(animated => animated == index);
    }

    public bool IsAnimated(AnimBone bone, AnimationType type)
    {
        if (bone == null) return false;
        return this.IsAnimated(bone.animHash, type);
    }

    public bool IsAnimated(int animHash, AnimationType type)
    {
        return this.GetAnimationIndex(animHash, type) != -1;
    }

    public int GetAnimationIndex(int animHash, AnimationType type)
    {
        byte[] indices = null;
        switch (type)
        {
            case AnimationType.ROTATION:
                indices = this.RotBonesAnimated;
                break;
            case AnimationType.POSITION:
                indices = this.PosBonesAnimated;
                break;
            case AnimationType.SCALE:
                indices = this.ScaledBonesAnimated;
                break;
        }

        if (indices == null) return -1;
        int boneIndex = this.GetBoneIndex(animHash);
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
        int index = this.GetBoneIndex(animHash);
        if (index == -1) return new Vector3();
        Vector4 translation;
        int animIndex = this.GetAnimationIndex(animHash, AnimationType.POSITION);
        if (frame == 0 || animIndex == -1) translation = this.PackedPosition[index].Value;
        else
            translation =
                this.PackedPosition[this.Bones.Length + ((frame - 1) * this.PosBonesAnimated.Length) + animIndex].Value;

        return new Vector3(translation.X, translation.Y, translation.Z);
    }

    public Quaternion GetRotationFrame(int animHash, int frame)
    {
        int index = this.GetBoneIndex(animHash);
        if (index == -1) return new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        Vector4 rotation;
        int animIndex = this.GetAnimationIndex(animHash, AnimationType.ROTATION);
        if (frame == 0 || animIndex == -1)
            rotation = this.PackedRotation[index].Value;
        else
            rotation =
                this.PackedRotation[this.Bones.Length + ((frame - 1) * this.RotBonesAnimated.Length) + animIndex].Value;

        return new Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W);
    }

    public Vector3 GetScaleFrame(int animHash, int frame)
    {
        int index = this.GetBoneIndex(animHash);
        if (index == -1) return new Vector3(1.0f, 1.0f, 1.0f);
        Vector4 scale;
        int animIndex = this.GetAnimationIndex(animHash, AnimationType.SCALE);
        if (frame == 0 || animIndex == -1)
            scale = this.PackedScale[index].Value;
        else
            scale =
                this.PackedScale[this.Bones.Length + ((frame - 1) * this.ScaledBonesAnimated.Length) + animIndex].Value;

        return new Vector3(scale.X, scale.Y, scale.Z);
    }

    private Vector4[] GetFrames(int animHash, AnimationType type)
    {
        int animIndex = this.GetAnimationIndex(animHash, type);
        if (animIndex == -1) return null;

        int boneIndex = this.GetBoneIndex(animHash);
        if (boneIndex == -1) return null;

        Vector4?[] pack = null;
        int animated = 0;
        switch (type)
        {
            case AnimationType.ROTATION:
            {
                pack = this.PackedRotation;
                animated = this.RotBonesAnimated.Length;
                break;
            }
            case AnimationType.POSITION:
            {
                pack = this.PackedPosition;
                animated = this.PosBonesAnimated.Length;
                break;
            }
            case AnimationType.SCALE:
            {
                pack = this.PackedScale;
                animated = this.ScaledBonesAnimated.Length;
                break;
            }
            default:
                return null;
        }

        Vector4[] frames = new Vector4[this.NumFrames];
        frames[0] = pack[boneIndex].Value;
        for (int i = 1; i < this.NumFrames; ++i)
            frames[i] = pack[this.Bones.Length + ((i - 1) * animated) + animIndex].Value;

        return frames;
    }

    public Vector4[] GetRotationFrames(AnimBone bone)
    {
        return this.GetRotationFrames(bone.animHash);
    }

    public Vector4[] GetRotationFrames(int animHash)
    {
        return this.GetFrames(animHash, AnimationType.ROTATION);
    }

    public Vector4[] GetPositionFrames(AnimBone bone)
    {
        return this.GetPositionFrames(bone.animHash);
    }

    public Vector4[] GetPositionFrames(int animHash)
    {
        return this.GetFrames(animHash, AnimationType.POSITION);
    }

    public Vector4[] GetScaleFrames(AnimBone bone)
    {
        return this.GetScaleFrames(bone.animHash);
    }

    public Vector4[] GetScaleFrames(int animHash)
    {
        return this.GetFrames(animHash, AnimationType.SCALE);
    }

    public float[] GetMorphFrames(int index)
    {
        if (index < 0 || index >= this.MorphCount || !this.IsAnimated(index)) return null;
        float[] frames = new float[this.NumFrames];
        frames[0] = this.PackedMorph[index];
        for (int i = 1; i < this.NumFrames; ++i)
            frames[i] =
                this.PackedMorph[this.MorphCount + ((i - 1) * this.MorphsAnimated.Length) + index];
        return frames;

    }

    
    public override SerializationData Build(Revision revision, byte compressionFlags)
    {
        Serializer serializer = new Serializer(this.GetAllocatedSize(), revision,
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
        if (this.Bones != null) size += (this.Bones.Length * AnimBone.BASE_ALLOCATION_SIZE);
        if (this.RotBonesAnimated != null) size += (this.RotBonesAnimated.Length * 2);
        if (this.PosBonesAnimated != null) size += (this.PosBonesAnimated.Length * 2);
        if (this.ScaledBonesAnimated != null) size += (this.ScaledBonesAnimated.Length * 2);
        if (this.MorphsAnimated != null) size += (this.MorphsAnimated.Length * 2);
        if (this.PackedRotation != null) size += (this.PackedRotation.Length * 0x10);
        if (this.PackedPosition != null) size += (this.PackedPosition.Length * 0x10);
        if (this.PackedScale != null) size += (this.PackedScale.Length * 0x10);
        if (this.PackedMorph != null) size += (this.PackedMorph.Length * 0x4);
        if (this.Locators != null) size += this.Locators.Sum(locator => (locator.GetAllocatedSize()));
        return size;
    }


}