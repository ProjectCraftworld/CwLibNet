using System.Drawing;
using System.Net.Mime;
using System.Numerics;
using CwLibNet.Enums;
using CwLibNet.Extensions;
using CwLibNet.IO.Streams;
using CwLibNet.Resources;
using CwLibNet.Singleton;
using CwLibNet.Structs.Animation;
using CwLibNet.Structs.Gmat;
using CwLibNet.Structs.Mesh;
using CwLibNet.Structs.StaticMesh;
using CwLibNet.Types;
using CwLibNet.Types.Data;
using CwLibNet.Util;
using SkiaSharp;
using VGltf;
using VGltf.Types;
using VGltf.Types.Extensions;
using VJson;
using VJson.Schema;
using Buffer = VGltf.Types.Buffer;
using Node = VGltf.Types.Node;

namespace CwLibNet.IO.Exports;

public class MeshExporter
{
    public class Glb
    {
        Gltf gltf = new Gltf();
        byte[]? buffer;

        Dictionary<String, int> bufferViews = new();
        Dictionary<String, int> materials = new();
        Dictionary<String, int> textures = new();
        int accessorCount = 0;

        public static Glb FromAnimation(RAnimation animation, RMesh mesh)
        {
            Glb glb;

            if (mesh == null)
            {
                glb = new Glb();
                byte[]? dataBuffer = glb.GetBufferFromAnimation(animation);
                VGltf.Types.Buffer buffer = new()
                {
                    ByteLength = dataBuffer.Length
                };
                glb.gltf.Buffers.Add(buffer);
                glb.buffer = dataBuffer;

                glb.SetAsset("CRAFTWORLD", "2.0");


                Node root = new Node();
                root.Name = ("Armature");
                root.Children = [1];
                root.Skin = 0;

                glb.gltf.Nodes.Add(root);

                for (int i = 0; i < animation.Bones.Length; ++i)
                {
                    Vector4 pos = animation.GetBasePosition(i).Value;
                    Vector4 rot = animation.GetBaseRotation(i).Value;
                    Vector4 scale = animation.GetBaseScale(i).Value;

                    Node child = new Node();

                    child.Translation = [pos.X, pos.Y, pos.Z];
                    child.Rotation = [rot.X, rot.Y, rot.Z, rot.W];
                    child.Scale = [scale.X, scale.Y, scale.Z];

                    glb.gltf.Nodes.Add(child);
                }

                Skin skin = new Skin();

                AnimBone[] animBones = animation.Bones;
                for (int i = 0; i < animBones.Length; ++i)
                {
                    skin.Joints = [..skin.Joints, i + 1];
                    if (animBones[i].parent != -1)
                        glb.gltf.Nodes[(animBones[i].parent + 1)].Children = [..glb.gltf.Nodes[(animBones[i].parent + 1)].Children, (i + 1)];
                }

                glb.gltf.Skins.Add(skin);
            }
            else
            {
                glb = Glb.FromMesh(mesh);
                byte[]? dataBuffer = glb.GetBufferFromAnimation(animation);
                glb.buffer = Bytes.Combine(glb.buffer, dataBuffer);
                glb.gltf.Buffers[0].ByteLength = (glb.buffer.Length);
                foreach (AnimBone bone in animation.Bones)
                {
                    String name = Bone.getNameFromHash(mesh.Bones, bone.animHash);
                    int boneIndex = animation.GetBoneIndex(bone.animHash);

                    Node? node = glb.GetNode(name);

                    Vector4 pos = animation.GetBasePosition(boneIndex).Value;
                    Vector4 rot = animation.GetBaseRotation(boneIndex).Value;
                    Vector4 scale = animation.GetBaseScale(boneIndex).Value;

                    node.Translation = ( [pos.X, pos.Y, pos.Z]);
                    node.Rotation = ( [rot.X, rot.Y, rot.Z, rot.W]);
                    node.Scale = ( [scale.X, scale.Y, scale.Z]);
                }
            }

            if (animation.MorphCount != 0)


                foreach (Node node in glb.gltf.Nodes)
                {
                    float[] weights = animation.GetBaseWeights();
                    List<float> gltfWeights = [];
                    gltfWeights.AddRange(weights.Select(weight => weight));
                    if (node.Mesh != null)
                        glb.gltf.Meshes[node.Mesh.Value].Weights = gltfWeights.ToArray();
                }

            int time = glb.CreateAccessor("TIME", 5126, "SCALAR", 0,
                animation.NumFrames);

            Animation glAnim = new Animation();

            int samplerIndex = 0;

            byte[] morphsAnimated = animation.MorphsAnimated;
            if (morphsAnimated.Length != 0)
            {
                for (int i = 0; i < glb.gltf.Nodes.Count; ++i)
                {
                    Node node = glb.gltf.Nodes[i];
                    if (node.Mesh == null) continue;
                    //AnimationChannelTarget target = new AnimationChannelTarget();
                    Animation.ChannelType channel = new Animation.ChannelType();
                    Animation.ChannelType.TargetType target = new Animation.ChannelType.TargetType
                    {
                        Node = i,
                        Path = Animation.ChannelType.TargetType.PathEnum.Weights
                    };
                    channel.Target = target;
                    Animation.SamplerType sampler = new Animation.SamplerType
                    {
                        Input = (time),
                        Interpolation = Animation.SamplerType.InterpolationEnum.LINEAR,
                        Output = (glb.CreateAccessor("MORPHS_ANIMATED", 5126,
                            "SCALAR", 0,
                            animation.MorphCount * (animation.NumFrames - 1)))
                    };
                    channel.Sampler = (samplerIndex);
                    samplerIndex++;
                    glAnim.Channels.Add(channel);
                    glAnim.Samplers.Add(sampler);
                }
            }

            AnimBone[] bones = animation.Bones;

            foreach (byte bone in animation.PosBonesAnimated)
            {
                int pos = bone & 0xff;

                Animation.ChannelType channel = new Animation.ChannelType();
                Animation.ChannelType.TargetType target = new Animation.ChannelType.TargetType();
                if (mesh != null)
                    target.Node = (glb.GetNodeIndex(Bone.getNameFromHash(mesh.Bones,
                        animation.Bones[pos].animHash)));
                target.Node = (pos + 1);
                target.Path = Animation.ChannelType.TargetType.PathEnum.Translation;
                channel.Target = (target);
                Animation.SamplerType sampler = new Animation.SamplerType();
                sampler.Input = (time);
                sampler.Interpolation = Animation.SamplerType.InterpolationEnum.LINEAR;
                sampler.Output = (glb.CreateAccessor("BONE_TRANSLATION_" + bones[pos].animHash,
                    5126, "VEC3", 0, animation.NumFrames));
                channel.Sampler = (samplerIndex);
                samplerIndex++;
                glAnim.Channels.Add(channel);
                glAnim.Samplers.Add(sampler);
            }

            foreach (byte bone in animation.ScaledBonesAnimated)
            {
                int scale = bone & 0xff;

                Animation.ChannelType channel = new Animation.ChannelType();
                Animation.ChannelType.TargetType target = new Animation.ChannelType.TargetType();
                if (mesh != null)
                    target.Node = (glb.GetNodeIndex(Bone.getNameFromHash(mesh.Bones,
                        animation.Bones[scale].animHash)));
                target.Node = (scale + 1);
                target.Path = Animation.ChannelType.TargetType.PathEnum.Scale;
                channel.Target = (target);
                Animation.SamplerType sampler = new Animation.SamplerType
                {
                    Input = (time),
                    Interpolation = Animation.SamplerType.InterpolationEnum.LINEAR,
                    Output = (glb.CreateAccessor("BONE_SCALE_" + bones[scale].animHash, 5126,
                        "VEC3", 0, animation.NumFrames))
                };
                channel.Sampler = (samplerIndex);
                samplerIndex++;
                glAnim.Channels.Add(channel);
                glAnim.Samplers.Add(sampler);
            }

            foreach (byte bone in animation.RotBonesAnimated)
            {
                int rot = bone & 0xff;

                Animation.ChannelType channel = new Animation.ChannelType();
                Animation.ChannelType.TargetType target = new Animation.ChannelType.TargetType();
                if (mesh != null)
                    target.Node = (glb.GetNodeIndex(Bone.getNameFromHash(mesh.Bones,
                        animation.Bones[rot].animHash)));
                else target.Node = (rot + 1);
                target.Path = Animation.ChannelType.TargetType.PathEnum.Rotation;
                channel.Target = (target);
                Animation.SamplerType sampler = new Animation.SamplerType
                {
                    Input = (time),
                    Interpolation = Animation.SamplerType.InterpolationEnum.LINEAR,
                    Output = (glb.CreateAccessor("BONE_ROTATION_" + bones[rot].animHash, 5126
                        , "VEC4", 0, animation.NumFrames))
                };
                channel.Sampler = (samplerIndex);
                samplerIndex++;
                glAnim.Channels.Add(channel);
                glAnim.Samplers.Add(sampler);
            }

            glb.gltf.Animations.Add(glAnim);

            return glb;
        }

        /* public static GLB FromMesh(RStaticMesh mesh)
        {
            GLB glb = new GLB();
            byte[] dataBuffer = glb.getBufferFromMesh(mesh);
            Buffer buffer = new Buffer();
            buffer.setByteLength(dataBuffer.length);
            glb.gltf.addBuffers(buffer);
            glb.buffer = dataBuffer;
            glb.setAsset("CRAFTWORLD", "2.0");

            Mesh glMesh = new Mesh();
            for (int i = 0; i < mesh.getMeshInfo().primitives.length; ++i)
            {
                StaticPrimitive primitive = mesh.getMeshInfo().primitives[i];
                MeshPrimitive glPrimitive = new MeshPrimitive();
                glPrimitive.addAttributes("POSITION",
                    glb.createAccessor(
                        "VERTICES",
                        5126,
                        "VEC3",
                        primitive.vertexStart * 0xC,
                        primitive.numVerts)
                );

                glPrimitive.addAttributes("NORMAL",
                    glb.createAccessor(
                        "NORMALS",
                        5126,
                        "VEC3",
                        primitive.vertexStart * 0xC,
                        primitive.numVerts)
                );

                for (int j = 0; j < 2; ++j)
                {
                    glPrimitive.addAttributes("TEXCOORD_" + j,
                        glb.createAccessor(
                            "TEXCOORD_" + j,
                            5126,
                            "VEC2",
                            primitive.vertexStart * 0x8,
                            primitive.numVerts)
                    );
                }

                glPrimitive.setIndices(
                    glb.createAccessor(
                        "INDICES_" + i,
                        5123,
                        "SCALAR",
                        0,
                        glb.gltf.getBufferViews().get(glb.getBufferView("INDICES_"
                                                                        + i)).getByteLength() / 2)
                );

                String materialName = "DIFFUSE";
                if (primitive.gmat != null)
                {
                    FileEntry entry = ResourceSystem.get(primitive.gmat);
                    if (entry != null)
                    {
                        materialName =
                            Paths.get(entry.getPath()).getFileName().toString().replaceFirst(
                                "[.][^.]+$", "");
                        try
                        {
                            byte[] data = ResourceSystem.extract(entry);
                            if (data != null)
                                glPrimitive.setMaterial(glb.createMaterial(materialName,
                                    new SerializedResource(data).loadResource(RGfxMaterial.class)));
                            else
                                glPrimitive.setMaterial(glb.createMaterial(materialName));
                        }
                        catch (Exception e)
                        {
                            glPrimitive.setMaterial(glb.createMaterial(materialName));
                        }
                    }
                    else
                    {
                        materialName = primitive.gmat.toString();
                        glPrimitive.setMaterial(glb.createMaterial(materialName));
                    }
                }

                glPrimitive.setMode(4);

                glMesh.addPrimitives(glPrimitive);
            }

            glb.gltf.addMeshes(glMesh);

            Node root = new Node();
            root.setName("bg");
            root.setMesh(0);

            glb.gltf.addNodes(root);
            glb.gltf.setScene(0);

            Scene scene = new Scene();
            scene.setName("Scene");
            scene.addNodes(0);
            glb.gltf.addScenes(scene);

            return glb;
        } */

        public static Glb FromMesh(RMesh mesh)
        {
            Glb glb = new Glb();


            byte[]? dataBuffer = glb.GetBufferFromMesh(mesh);
            Buffer buffer = new Buffer
            {
                ByteLength = (dataBuffer.Length)
            };
            glb.gltf.Buffers.Add(buffer);
            glb.buffer = dataBuffer;

            glb.SetAsset("CRAFTWORLD", "2.0");

            Primitive[][] subMeshes = mesh.GetSubmeshes();
            for (int m = 0; m < subMeshes.Length; ++m)
            {
                Mesh glMesh = new Mesh();
                if (mesh.MorphCount != 0)
                {
                    glMesh.Extras = new();
                    StringNode[] morphs = new StringNode[mesh.MorphCount];
                    for (int i = 0; i < morphs.Length; ++i)
                        morphs[i] = new StringNode(mesh.MorphNames[i]);
                    glMesh.Extras.Add("targetNames", new ArrayNode(morphs.ToList<INode>()));
                }
                for (int i = 0; i < subMeshes[m].Length; ++i)
                {
                    Primitive primitive = subMeshes[m][i];
                    Mesh.PrimitiveType glPrimitive = new Mesh.PrimitiveType();

                    glPrimitive.Attributes.Add("POSITION",
                        glb.CreateAccessor(
                            "VERTICES",
                            5126,
                            "VEC3",
                            primitive.MinVert * 0xC,
                            primitive.MaxVert - primitive.MinVert + 1)
                    );

                    glPrimitive.Attributes.Add("NORMAL",
                        glb.CreateAccessor(
                            "NORMAL",
                            5126,
                            "VEC3",
                            primitive.MinVert * 0xC,
                            primitive.MaxVert - primitive.MinVert + 1)
                    );

                    for (int j = 0; j < mesh.AttributeCount; ++j)
                    {
                        glPrimitive.Attributes.Add("TEXCOORD_" + j,
                            glb.CreateAccessor(
                                "TEXCOORD_" + j,
                                5126,
                                "VEC2",
                                primitive.MinVert * 0x8,
                                primitive.MaxVert - primitive.MinVert + 1)
                        );
                    }

                    if (mesh.MorphCount != 0)
                    {
                        for (int j = 0; j < mesh.MorphCount; ++j)
                        {
                            Dictionary<String, int> target = new Dictionary<string, int>();
                            target.Add("POSITION", glb.CreateAccessor(
                                "MORPH_" + j,
                                5126,
                                "VEC3",
                                primitive.MinVert * 0xC,
                                primitive.MaxVert - primitive.MinVert + 1)
                            );
                            target.Add("NORMAL", glb.CreateAccessor(
                                "MORPH_NORMAL_" + j,
                                5126,
                                "VEC3",
                                primitive.MinVert * 0xC,
                                primitive.MaxVert - primitive.MinVert + 1)
                            );
                            glPrimitive.Targets.Add(target);
                        }
                    }

                    glPrimitive.Attributes.Add("JOINTS_0",
                        glb.CreateAccessor(
                            "JOINTS",
                            5121,
                            "VEC4",
                            primitive.MinVert * 0x4,
                            primitive.MaxVert - primitive.MinVert + 1
                        )
                    );

                    glPrimitive.Attributes.Add("WEIGHTS_0",
                        glb.CreateAccessor(
                            "WEIGHTS",
                            5126,
                            "VEC4",
                            primitive.MinVert * 0x10,
                            primitive.MaxVert - primitive.MinVert + 1
                        )
                    );

                    glPrimitive.Attributes.Add("COLOR_0",
                        glb.CreateAccessor(
                            "COLOR",
                            5123,
                            "VEC4",
                            primitive.MinVert * 0x8,
                            primitive.MaxVert - primitive.MinVert + 1
                        )
                    );

                    glPrimitive.Indices = (
                        glb.CreateAccessor(
                            "INDICES_" + m + "_" + i,
                            5123,
                            "SCALAR",
                            0,
                            glb.gltf.BufferViews[(glb.GetBufferView(
                                "INDICES_" + m + "_" + i))].ByteLength / 2)
                    );

                    String materialName = "DIFFUSE";
                    if (primitive.Material != null)
                    {
                        FileEntry? entry = ResourceSystem.Get(primitive.Material);
                        if (entry != null)
                        {
                            materialName =
                                Path.GetFileName(entry.Value.Path).Replace(
                                    "[.][^.]+$", "");
                            try
                            {
                                byte[]? data = ResourceSystem.Extract(primitive.Material);
                                if (data != null)
                                    glPrimitive.Material = (glb.CreateMaterial(materialName,
                                        new SerializedResource(data).loadResource<RGfxMaterial>()));
                                else
                                    glPrimitive.Material = (glb.CreateMaterial(materialName));
                            }
                            catch (Exception e)
                            {
                                glPrimitive.Material = (glb.CreateMaterial(materialName));
                            }
                        }
                        else
                        {
                            materialName = primitive.Material.ToString();
                            glPrimitive.Material = (glb.CreateMaterial(materialName));
                        }
                    }

                    glPrimitive.Mode = Mesh.PrimitiveType.ModeEnum.TRIANGLES;

                    glMesh.Primitives.Add(glPrimitive);
                }

                glb.gltf.Meshes.Add(glMesh);
            }

            Node root = new Node();
            root.Name = ("Armature");

            glb.CreateSkeleton(mesh);
            Skin skin = new Skin();
            foreach (Bone bone in mesh.Bones)
            {
                int index = glb.GetNodeIndex(bone.getName());
                skin.Joints = [..skin.Joints, (index)];
                if (bone.parent == -1)
                    root.Children = [..root.Children, (index)];
            }
            skin.InverseBindMatrices = (glb.CreateAccessor("MATRIX", 5126, "MAT4", 0,
                mesh.Bones.Length));
            glb.gltf.Skins.Add(skin);

            for (int i = 0; i < subMeshes.Length; ++i)
            {
                Node child = new Node();
                child.Mesh = i;
                child.Skin = (0);
                glb.gltf.Nodes.Add(child);
                root.Children = [..root.Children, (glb.gltf.Nodes.Count - 1)];
            }

            glb.gltf.Nodes.Add(root);

            glb.gltf.Scene = (0);

            Scene scene = new Scene();
            scene.Name = ("Scene");
            scene.Nodes = [..scene.Nodes, (glb.gltf.Nodes.Count - 1)];
            glb.gltf.Scenes.Add(scene);

            return glb;
        }

        private int AddTexture(String name, byte[] buffer)
        {
            if (GetBufferView("TEXTURE_" + name) != -1) return this.textures[(name)];
            Image image = new()
            {
                BufferView = (CreateBufferView("TEXTURE_" + name, 0, buffer.Length)),
                MimeType = ("image/png"),
                Name = (name)
            };
            this.buffer = Bytes.Combine(this.buffer, buffer);
            this.gltf.Buffers[0].ByteLength = (this.buffer.Length);
            Texture texture = new();
            this.gltf.Images.Add(image);
            texture.Source = (this.gltf.Images.Count - 1);
            texture.Name = (name);
            this.gltf.Textures.Add(texture);
            int index = this.gltf.Textures.Count - 1;
            this.textures.Add(name, index);
            return index;
        }

        private void CreateSkeleton(RMesh mesh)
        {
            // mesh.Bones;
            foreach (Bone bone in mesh.Bones)
            {
                if (bone.parent == -1)
                    CreateChildren(mesh, bone);
            }
        }

        private int CreateChildren(RMesh mesh, Bone bone)
        {
            Matrix4x4 transform = bone.getLocalTransform(mesh.Bones).Value;
            int index = CreateNode(bone.getName(), transform);
            Node root = this.gltf.Nodes[index];

            Bone[] children = bone.getChildren(mesh.Bones);

            foreach (Bone child in children)
            {
                root.Children = [..root.Children, (CreateChildren(
                    mesh,
                    child
                ))];
            }
            return index;
        }

        private int CreateNode(String name, Matrix4x4 transform)
        {
            Node node = new Node();
            node.Name = (name);

            Vector3 translation = transform.GetTranslation();
            node.Translation = ( [translation.X, translation.Y, translation.Z]);

            Quaternion rotation = new Quaternion();
            rotation.SetFromUnnormalized(transform);
            node.Rotation = ( [rotation.X, rotation.Y, rotation.Z, rotation.W]);

            Vector3 scale = transform.GetScale();
            node.Scale = ( [scale.X, scale.Y, scale.Z]);

            this.gltf.Nodes.Add(node);
            return this.gltf.Nodes.Count - 1;
        }

        private Node? GetNode(String name)
        {
            List<Node?> nodes = this.gltf.Nodes;
            return nodes.FirstOrDefault(node => node.Name.Equals(name));
        }

        private int GetNodeIndex(string? name)
        {
            List<Node> nodes = this.gltf.Nodes;
            for (int i = 0; i < nodes.Count; ++i)
                if (nodes[i].Name.Equals(name))
                    return i;
            return -1;
        }

        public byte[] ConvertTexture(RGfxMaterial gfx, int index)
        {
            ResourceDescriptor descriptor = gfx.Textures[index];
            if (descriptor == null) return null;
            byte[] data = ResourceSystem.Extract(descriptor);
            if (data == null) return null;
            RTexture texture = null;
            try
            {
                texture = new RTexture(data);
                return texture.getImage().Encode(SKEncodedImageFormat.Png, 100).AsSpan().ToArray();
            }
            catch (Exception ex) { return null; }
        }

        public class KhrTextureTransform
        {
            public float[] Offset;
            public float Rotation;
            public float[] Scale;
            // public int texCoord;

            public INode ToNode()
            {
                return new ObjectNode(new Dictionary<string, INode>()
                {
                    ["offset"] = new ArrayNode(Offset.Select(o => new FloatNode(o)).ToList<INode>()),
                    ["rotation"] = (new FloatNode(Rotation)),
                    ["scale"] = new ArrayNode(Scale.Select(s => new FloatNode(s)).ToList<INode>()),
                });
            }
        }

        private int CreateMaterial(String name, RGfxMaterial gmat)
        {
            if (this.materials.TryGetValue(name, out var material1))
                return material1;

            Material material = new Material
            {
                Name = name,
                DoubleSided = true
            };

            Material.PbrMetallicRoughnessType pbr = new Material.PbrMetallicRoughnessType();

            bool foundDiffuse = false;
            bool foundBump = false;
            int outputBox = gmat.GetOutputBox();
            for (int i = 0; i < gmat.Boxes.Count; ++i)
            {
                if (foundBump && foundDiffuse) break;
                MaterialBox box = gmat.Boxes[i];
                /*
                
                DIFFUSE : 0,
                SPECULAR: 2,
                BUMP: 3,
                GLOW: 4,
                REFLECTION: 6
                */

                if (box.Type == BoxType.TEXTURE_SAMPLE)
                {
                    int[] @params = box.GetParameters();

                    float[] textureScale, textureOffset;
                    float textureRotation;
                    if (box.SubType == 1)
                    {
                        Vector2 col0 =
                            new Vector2(@params[0].IntBitsToFloat(),
                                @params[1].IntBitsToFloat());
                        Vector2 col1 =
                            new Vector2(@params[3].IntBitsToFloat(),
                                @params[6].IntBitsToFloat());

                        textureScale =
                        [
                            col0.Length(),
                            col1.Length()
                        ];

                        col0 /= (col0.Length());
                        col1 /= (col1.Length());


                        textureOffset =
                        [
                            @params[2].IntBitsToFloat(),
                                @params[7].IntBitsToFloat()];
                        textureRotation = (float) Math.Acos(col0.X);

                    }
                    else
                    {
                        textureScale =
                        [
                            @params[0].IntBitsToFloat(),
                                @params[1].IntBitsToFloat()
                        ];
                        textureOffset =
                        [
                            @params[2].IntBitsToFloat(),
                                @params[3].IntBitsToFloat()
                        ];
                        textureRotation = 0.0f;
                    }
                    int channel = @params[4];
                    int textureIndex = @params[5];
                    FileEntry? entry = ResourceSystem.Get(gmat.Textures[textureIndex]);
                    if (entry == null) continue;
                    byte[] texture = this.ConvertTexture(gmat, textureIndex);
                    if (texture == null) continue;
                    int source =
                        AddTexture(Path.GetFileNameWithoutExtension(entry.Value.Path), texture);
                    SKBitmap image = SKBitmap.FromImage(SKImage.FromEncodedData(texture));
                    KhrTextureTransform transforms = new KhrTextureTransform();
                    transforms.Offset = textureOffset;
                    transforms.Scale = textureScale;
                    transforms.Rotation = textureRotation;


                    MaterialWire wire = gmat.FindWireFrom(i);
                    while (wire.BoxTo != outputBox)
                        wire = gmat.FindWireFrom(wire.BoxTo);

                    Material.BaseColorTextureInfoType textureInfo = new Material.BaseColorTextureInfoType();
                    textureInfo.Extensions.Add("KHR_texture_transform", transforms.ToNode());
                    textureInfo.TexCoord = (channel);
                    textureInfo.Index = (source);
                    switch (wire.PortTo)
                    {
                        case 0:
                            if (entry.Value.Path.Contains("dirt"))
                            {
                                Material.OcclusionTextureInfoType occInfo =
                                    new Material.OcclusionTextureInfoType();
                                occInfo.Extensions.Add("KHR_texture_transform",
                                    transforms.ToNode());
                                occInfo.Index = (source);
                                occInfo.TexCoord = channel;
                                material.OcclusionTexture = (occInfo);
                                continue;
                            }
                            if (foundDiffuse) continue;
                            /* System.out.printf("%s:%d%n",
                                Paths.get(entry.getPath()).getFileName().toString().replaceFirst(
                                    "[.][^.]+$", ""), image.getTransparency()); */
                            if (name.ToLower().Contains("decal"))
                                material.AlphaMode = Material.AlphaModeEnum.Blend;
                            foundDiffuse = true;
                            pbr.BaseColorTexture = (textureInfo);
                            if (material.Extensions != null && material.Extensions.TryGetValue("KHR_materials_pbrSpecularGlossiness", out var extension))
                            {
                                ObjectNode map =
                                    (ObjectNode) extension;
                                if (!map.Elems.ContainsKey("diffuseTexture"))
                                    map.AddElement("diffuseTexture", new ObjectNode(new Dictionary<string, INode>()
                                    {
                                        ["index"] = (new IntegerNode(textureInfo.Index)),
                                    }));
                            }
                            continue;
                        case 2:
                            /*
                            HashMap<String, TextureInfo> extension = new HashMap<String,
                            TextureInfo>();
                            extension.put("specularGlossinessTexture", textureInfo);
                            material.addExtensions("KHR_materials_pbrSpecularGlossiness",
                            extension);
                            */
                            continue;
                        case 3:
                            if (foundBump) continue;
                            foundBump = true;

                            if (image != null)
                            {
                                for (int x = 0; x < image.Width; ++x)
                                {
                                    for (int y = 0; y < image.Height; ++y)
                                    {
                                        SKColor c = image.GetPixel(x, y);

                                        SKColor output =
                                            new SKColor((byte)(255 - c.Alpha),
                                                c.Green
                                                , 255, 255);

                                        image.SetPixel(x, y, output);
                                    }
                                }
                                byte[] bytes = image.Encode(SKEncodedImageFormat.Png, 100).ToArray();

                                source =
                                    AddTexture(Path.GetFileNameWithoutExtension(entry.Value.Path) + "_converted", bytes);
                            }

                            Material.NormalTextureInfoType normal =
                                new Material.NormalTextureInfoType();
                            normal.Extensions.Add("KHR_texture_transform", transforms.ToNode());
                            normal.Index = (source);
                            normal.TexCoord = (channel);
                            material.NormalTexture = (normal);
                            continue;

                    }
                }
                else if (box.Type == BoxType.COLOR)
                {
                    MaterialWire wire = gmat.FindWireFrom(i);
                    if (wire.BoxTo == outputBox)
                    {
                        int[] @params = box.GetParameters();
                        float[] color =
                        [
                            @params[0].IntBitsToFloat() / 255f,
                                @params[1].IntBitsToFloat() / 255f,
                                @params[2].IntBitsToFloat() / 255f,
                                @params[3].IntBitsToFloat() / 255f
                        ];
                        if (wire.PortTo == 0)
                        {
                            if (material.Extensions != null && material.Extensions.ContainsKey("KHR_materials_pbrSpecularGlossiness"))
                            {
                                Dictionary<String, INode> map =
                                    (Dictionary<String, INode>) material.Extensions[
                                        "KHR_materials_pbrSpecularGlossiness"];
                                if (!map.ContainsKey("diffuseFactor"))
                                    map.Add("diffuseFactor", new ArrayNode(color.Select(e => new FloatNode(e)).ToList<INode>()));
                            }
                            foundDiffuse = true;
                            pbr.BaseColorFactor = (color);
                        }
                        else if (wire.PortTo == 2)
                        {
                            Dictionary<String, INode> extension = new Dictionary<string, INode>
                            { { "specularFactor", new ArrayNode(new[] { color[0],
                                color[1],
                                color[2] }.Select(e => new FloatNode(e)).ToList<INode>()) } };
                            if (pbr.BaseColorFactor != null)
                                extension.Add("diffuseFactor",
                                    new ArrayNode(pbr.BaseColorFactor.Select(e => new FloatNode(e)).ToList<INode>()));
                            if (pbr.BaseColorTexture != null)
                                extension.Add("diffuseTexture",
                                    new ObjectNode(new Dictionary<string, INode>()
                                    {
                                        ["index"] = new IntegerNode(pbr.BaseColorTexture.Index),
                                    }));
                            material.Extensions?.Add(
                                "KHR_materials_pbrSpecularGlossiness",
                                new ObjectNode(extension));
                        }
                    }
                }
            }

            if (!foundDiffuse)
                pbr.BaseColorFactor = ( [1.0f, 1.0f, 1.0f, 1.0f]);

            material.PbrMetallicRoughness = pbr;

            this.gltf.AddMaterial(material);
            int index = this.materials.Count;
            this.materials.Add(name, index);
            return index;
        }

        private int CreateMaterial(String name)
        {
            if (this.materials.TryGetValue(name, out var material1))
                return material1;

            Material material = new Material
            {
                Name = (name)
            };
            Material.PbrMetallicRoughnessType pbr = new Material.PbrMetallicRoughnessType
            {
                BaseColorFactor = ( [1.0f, 1.0f, 1.0f, 1.0f])
            };
            material.PbrMetallicRoughness = (pbr);

            this.gltf.AddMaterial(material);
            int index = this.materials.Count;
            this.materials.Add(name, index);
            return index;
        }

        private void SetAsset(String generator, String version)
        {
            Asset asset = new Asset();
            asset.Generator = (generator);
            asset.Version = (version);
            this.gltf.Asset = (asset);
        }

        private int CreateAccessor(String bufferView, int componentType, String type,
                                   int offset,
                                   int count)
        {
            Accessor accessor = new Accessor();
            accessor.BufferView = (GetBufferView(bufferView));
            accessor.ByteOffset = (offset);
            accessor.ComponentType = (Accessor.ComponentTypeEnum)(componentType);
            accessor.Type = (Accessor.TypeEnum)Enum.Parse(typeof(Accessor.TypeEnum), type);
            accessor.Count = (count);
            this.accessorCount++;
            this.gltf.Accessors.Add(accessor);
            return this.accessorCount - 1;
        }

        private int CreateBufferView(String name, int offset, int length)
        {
            if (this.buffer != null)
                offset += this.buffer.Length;
            BufferView view = new BufferView
            {
                Buffer = 0,
                ByteOffset = offset,
                ByteLength = length
            };
            this.gltf.BufferViews.Add(view);
            int index = this.bufferViews.Count;
            this.bufferViews.Add(name, index);
            return index;
        }

        public int GetBufferView(String name)
        {
            return this.bufferViews.GetValueOrDefault(name, -1);
        }

        private byte[]? GetBufferFromAnimation(RAnimation animation)
        {
            float timestep = 1.0f / ((float) animation.Fps);

            MemoryOutputStream output =
                new MemoryOutputStream(animation.NumFrames * animation.Fps * animation.Bones.Length + (animation.PosBonesAnimated.Length + animation.RotBonesAnimated.Length + animation.ScaledBonesAnimated.Length) * 0x50 + 0xFF0);
            output.SetLittleEndian(true);

            float step = 0.0f;
            for (int i = 0; i < animation.NumFrames; ++i, step += timestep)
                output.F32(step);
            CreateBufferView("TIME", 0, output.GetOffset());
            AnimBone[] bones = animation.Bones;
            for (int i = 0; i < bones.Length; ++i)
            {
                AnimBone bone = bones[i];

                if (animation.IsAnimated(bone, RAnimation.AnimationType.POSITION))
                {
                    int posStart = output.GetOffset();
                    foreach (Vector4 pos in animation.GetPositionFrames(bone))
                    {
                        output.F32(pos.X);
                        output.F32(pos.Y);
                        output.F32(pos.Z);
                    }
                    CreateBufferView("BONE_TRANSLATION_" + bone.animHash,
                        posStart, output.GetOffset() - posStart);
                }

                if (animation.IsAnimated(bone, RAnimation.AnimationType.ROTATION))
                {
                    int rotStart = output.GetOffset();
                    foreach (Vector4 rot in animation.GetRotationFrames(bone))
                    {
                        output.F32(rot.X);
                        output.F32(rot.Y);
                        output.F32(rot.Z);
                        output.F32(rot.W);
                    }
                    CreateBufferView("BONE_ROTATION_" + bone.animHash,
                        rotStart,
                        output.GetOffset() - rotStart);
                }

                if (animation.IsAnimated(bone, RAnimation.AnimationType.SCALE))
                {
                    int scaleStart = output.GetOffset();
                    foreach (Vector4 scale in animation.GetScaleFrames(bone))
                    {
                        output.F32(scale.X);
                        output.F32(scale.Y);
                        output.F32(scale.Z);
                    }
                    CreateBufferView("BONE_SCALE_" + bone.animHash,
                        scaleStart,
                        output.GetOffset() - scaleStart);
                }
            }

            if (animation.MorphsAnimated.Length != 0)
            {
                float[][] morphFrames = new float[animation.MorphCount][];
                for (int i = 0; i < animation.MorphCount; ++i)
                {
                    if (animation.IsAnimated(i))
                        morphFrames[i] = animation.GetMorphFrames(i);
                    else
                    {
                        float[] weights = new float[animation.NumFrames];
                        float @base = animation.GetBaseWeight(i);
                        for (int j = 0; j < weights.Length; ++j)
                            weights[j] = @base;
                    }
                }

                int morphStart = output.GetOffset();
                for (int i = 0; i < animation.NumFrames; ++i)
                {
                    for (int j = 0; j < animation.MorphCount; ++j)
                        output.F32(morphFrames[j][i]);
                }
                CreateBufferView("MORPHS_ANIMATED", morphStart,
                    output.GetOffset() - morphStart);
            }

            output.Shrink();
            return output.GetBuffer();

        }

        /* private byte[] getBufferFromMesh(RStaticMesh mesh)
        {
            MemoryOutputStream output =
                new MemoryOutputStream(mesh.getNumVerts() * 0x80 + ((mesh.getNumVerts() - 1) * 0x8));
            output.SetLittleEndian(true);

            foreach (Vector3 vertex in mesh.getVertices())
            {
                output.F32(vertex.X);
                output.F32(vertex.Y);
                output.F32(vertex.Z);
            }
            createBufferView("VERTICES", 0, output.GetOffset());
            int normalStart = output.GetOffset();
            foreach (Vector3 normal in mesh.GetNormals())
            {
                output.F32(normal.X);
                output.F32(normal.Y);
                output.F32(normal.Z);
            }
            createBufferView("NORMALS", normalStart, output.GetOffset() - normalStart);
            int uvStart = output.GetOffset();
            foreach (Vector2 uv in mesh.getUV0())
            {
                output.F32(uv.X);
                output.F32(uv.Y);
            }
            createBufferView("TEXCOORD_0", uvStart, output.GetOffset() - uvStart);
            uvStart = output.GetOffset();
            foreach (Vector2 uv in mesh.getUV1())
            {
                output.F32(uv.X);
                output.F32(uv.Y);
            }
            createBufferView("TEXCOORD_1", uvStart, output.GetOffset() - uvStart);
            StaticPrimitive[] primitives = mesh.getMeshInfo().primitives;
            for (int i = 0; i < primitives.Length; ++i)
            {
                StaticPrimitive primitive = primitives[i];
                int primitiveStart = output.GetOffset();
                int[] triangles = mesh.getTriangles(primitive.indexStart,
                    primitive.numIndices,
                    primitive.type);
                primitive.numVerts = getMax(triangles) + 1;
                foreach (int triangle in triangles)
                    output.U16((short) triangle);
                createBufferView("INDICES_" + i, primitiveStart,
                    output.GetOffset() - primitiveStart);
            }
            output.Shrink();
            return output.GetBuffer();
        } */

        private byte[]? GetBufferFromMesh(RMesh mesh)
        {
            MemoryOutputStream output =
                new MemoryOutputStream((mesh.NumVerts * 0x50) + ((mesh.NumVerts - 1) * 8) + (mesh.AttributeCount * mesh.NumVerts * 8) + (mesh.MorphCount * mesh.NumVerts * 0x18) + (mesh.Bones.Length * 0x40));
            output.SetLittleEndian(true);

            foreach (Vector3 vertex in mesh.GetVertices())
            {
                output.F32(vertex.X);
                output.F32(vertex.Y);
                output.F32(vertex.Z);
            }
            CreateBufferView("VERTICES", 0, output.GetOffset());

            Primitive[][] subMeshes = mesh.GetSubmeshes();
            for (int i = 0; i < subMeshes.Length; ++i)
            {
                for (int j = 0; j < subMeshes[i].Length; ++j)
                {
                    int triangleStart = output.GetOffset();
                    Primitive primitive
                        = subMeshes[i][j];
                    int[] triangles = mesh.GetTriangles(primitive);

                    primitive.SetMinMax(
                        GetMin(triangles),
                        GetMax(triangles)
                    );

                    foreach (int triangle in triangles)
                        output.U16((short) (triangle - primitive.MinVert));
                    CreateBufferView("INDICES_" + i + "_" + j,
                        triangleStart, output.GetOffset() - triangleStart);
                }
            }

            int normalStart = output.GetOffset();
            Vector3[] normals = mesh.GetNormals();
            foreach (Vector3 normal in normals)
            {
                output.F32(normal.X);
                output.F32(normal.Y);
                output.F32(normal.Z);
            }
            CreateBufferView("NORMAL", normalStart, output.GetOffset() - normalStart);
            for (int i = 0; i < mesh.AttributeCount; ++i)
            {
                int uvStart = output.GetOffset();
                foreach (Vector2 texCoord in mesh.GetUVs(i))
                {
                    output.F32(texCoord.X);
                    output.F32(texCoord.Y);
                }
                CreateBufferView("TEXCOORD_" + i, uvStart,
                    output.GetOffset() - uvStart);
            }
            if (mesh.MorphCount != 0)
            {
                Morph[] morphs = mesh.GetMorphs();
                for (int i = 0; i < mesh.MorphCount; ++i)
                {
                    int morphStart = output.GetOffset();
                    Morph morph = morphs[i];
                    foreach (Vector3 vertex in morph.GetOffsets())
                    {
                        output.F32(vertex.X);
                        output.F32(vertex.Y);
                        output.F32(vertex.Z);
                    }
                    CreateBufferView("MORPH_" + i, morphStart,
                        output.GetOffset() - morphStart);
                }

                for (int i = 0; i < mesh.MorphCount; ++i)
                {
                    int morphStart = output.GetOffset();
                    Morph morph = morphs[i];
                    for (int j = 0; j < mesh.NumVerts; ++j)
                    {
                        Vector3 vertex = morph.GetNormals()[j];
                        output.F32(vertex.X - normals[j].X);
                        output.F32(vertex.Y - normals[j].Y);
                        output.F32(vertex.Z - normals[j].Z);
                    }

                    CreateBufferView("MORPH_NORMAL_" + i, morphStart,
                        output.GetOffset() - morphStart);
                }
            }

            if (output.GetOffset() % 0x40 != 0)
                output.Seek((0x40 - (output.GetOffset() % 0x40)));
            int matrixStart = output.GetOffset();
            foreach (var t in mesh.Bones)
            {
                foreach (float v in t.invSkinPoseMatrix?.Linearize())
                    output.F32(v);
            }
            CreateBufferView("MATRIX", matrixStart, output.GetOffset() - matrixStart);

            int jointStart = output.GetOffset();
            foreach (byte[] joints in mesh.GetJoints())
                for (int i = 0; i < 4; ++i)
                    output.I8(joints[i]);

            CreateBufferView("JOINTS", jointStart, output.GetOffset() - jointStart);

            int weightStart = output.GetOffset();
            foreach (Vector4 weight in mesh.GetWeights())
            {
                output.F32(weight.X);
                output.F32(weight.Y);
                output.F32(weight.Z);
                output.F32(weight.W);
            }

            CreateBufferView("WEIGHTS", weightStart, output.GetOffset() - weightStart);

            int colorStart = output.GetOffset();
            foreach (float weight in mesh.GetSoftbodyWeights(0, mesh.NumVerts))
            {
                ushort c = (ushort) Math.Round(weight * 0xFFFF);
                output.I16(c);
                output.I16(c);
                output.I16(c);
                output.I16(0xFFFF);
            }
            CreateBufferView("COLOR", colorStart, output.GetOffset() - colorStart);

            output.Shrink();

            return output.GetBuffer();
        }

        public static int GetMin(int[] triangles)
        {
            int minValue = triangles[0];
            for (int i = 1; i < triangles.Length; ++i)
                if (triangles[i] < minValue)
                    minValue = triangles[i];
            return minValue;
        }

        public static int GetMax(int[] triangles)
        {
            int maxValue = triangles[0];
            for (int i = 1; i < triangles.Length; ++i)
                if (triangles[i] > maxValue)
                    maxValue = triangles[i];
            return maxValue;
        }

        public void Export(String path)
        {
            using FileStream stream = new FileStream(path, FileMode.Create);
            GltfWriter.Write(stream, gltf, new JsonSchemaRegistry());
        }
    }
}