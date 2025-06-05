# CwLibNet Migration Plan

## Purpose
This document outlines the findings, differences, and migration strategy for overhauling the scripts from the legacy Craftworld project to the new CwLibNet_4_HUB structure.

## Key Findings (RMesh Example)
- **Modernization:** CwLibNet_4_HUB uses modern C# features and is engine-agnostic, while the legacy code is more Unity-focused but still compatible with .NET Standard 2.1.
- **Serialization:** The new codebase uses a robust, versioned serialization system with explicit revision checks. The legacy code uses a more procedural approach.
- **Data Structures:** The new codebase is modular, with clear separation of mesh data, serialization, and utility logic.
- **Extensibility:** The new code is easier to extend and maintain due to its modularity and clear separation.

## Updated Migration Goals
- Refactor scripts in `CwLibNet_4_HUB` to be compatible with the serialization patterns and code used in the legacy Craftworld HUB (LittleBIGPlanet 2 PC port).
- Ensure that serialization logic, interfaces, and data structures in `CwLibNet_4_HUB` match or interoperate with those in Craftworld HUB.
- Adapt or wrap CwLibNet_4_HUB's modular, instance-based serialization to support the static/global, return-code-based, and Unity-centric serialization style of Craftworld HUB where necessary.
- Maintain .NET Standard 2.1 compatibility for Unity and cross-platform use.
- Document all changes and patterns to ensure maintainability and clarity for future development.

---

## Migration Steps
1. Analyze each script for Unity dependencies and legacy patterns.
2. Document required changes and uncertainties in this file.
3. Develop or use automation tools (e.g., Python scripts) for repetitive changes.
4. Manually refactor complex or unique logic as needed.
5. Test migrated scripts for compatibility and correctness.

## Open Questions / To Investigate
- Which Unity-specific features (if any) must be retained for compatibility?
- Are there any custom serialization edge cases?
- How should we handle scripts with heavy editor or runtime logic?

## Next Steps
- Continue analyzing additional scripts and update this document with new findings.
- Define specific code patterns to automate.
- Plan the automation/refactoring workflow.

## Detailed Comparison: RMesh.cs (Legacy vs. CwLibNet_4_HUB)

### 1. Namespaces & Dependencies
- **Legacy:** Uses UnityEngine, UnityEditor, and Unity-specific types (e.g., `Mesh`, `Material`, `Vector3`).
- **CwLibNet_4_HUB:** Uses `System.Numerics` for math, custom namespaces for enums, structs, and IO. No Unity dependencies.

### 2. Serialization
- **Legacy:**
  - Uses `Serializer.Serialize(ref field)` for each field.
  - Procedural, less modular, and less version-aware.
  - Serialization logic is mixed with Unity logic.
- **CwLibNet_4_HUB:**
  - Uses a versioned, revision-aware `Serializer` with methods like `I32`, `Str`, `Floatarray`, `Enumarray`, etc.
  - Handles new/old fields based on version.
  - Serialization is modular and separated from engine logic.

### 3. Data Structures
- **Legacy:**
  - Mesh data, bones, morphs, etc., are all public fields, often Unity types.
  - Some fields are Unity-specific (e.g., `Mesh mesh`, `Material[] materials`).
- **CwLibNet_4_HUB:**
  - Uses custom classes/structs for mesh data, bones, morphs, etc.
  - More explicit and modular, with clear separation of mesh, bone, and morph logic.

### 4. Methods & Logic
- **Legacy:**
  - Contains Unity-specific methods for mesh setup, rendering, and editor integration.
  - Methods like `setMesh`, `setVertexData`, and Unity event methods (`Update`).
- **CwLibNet_4_HUB:**
  - Focuses on data access, manipulation, and serialization.
  - No Unity event methods or rendering logic.
  - Methods are more granular (e.g., `GetVertices`, `GetNormals`, `GetWeights`).

### 5. Extensibility & Maintainability
- **Legacy:**
  - Tightly coupled to Unity and game runtime.
  - Harder to extend for non-Unity use cases.
- **CwLibNet_4_HUB:**
  - Engine-agnostic, modular, and easier to maintain or extend.
  - Designed for use as a general-purpose library.

### 6. Versioning & Compatibility
- **Legacy:**
  - Minimal versioning logic in serialization.
- **CwLibNet_4_HUB:**
  - Explicit version and revision checks for serialization and field handling.

### 7. Return Types & Error Handling
- **Legacy:**
  - Uses `SERIALIZER_RESULT` enums and explicit return values for serialization methods (e.g., `SERIALIZER_RESULT.OK`).
  - Methods like `Serialize`, `SerializeToResource` return status codes.
  - Error handling is more manual and explicit.
- **CwLibNet_4_HUB:**
  - Serialization methods are `void` or return data directly; no `SERIALIZER_RESULT` or explicit status codes.
  - Error handling is more idiomatic C# (exceptions, null checks, etc.).
  - Focuses on throwing exceptions for invalid states rather than returning error codes.

---

*Continue to add more detailed field-by-field or method-by-method differences as you analyze further!*

## Detailed Comparison: RAnimation.cs (Legacy vs. CwLibNet_4_HUB)

### 1. Namespaces & Dependencies
- **Legacy:** Uses Unity types (`Vector3`, `Quaternion`, `Matrix4x4`), Unity serialization, and Unity-specific logic.
- **CwLibNet_4_HUB:** Uses `System.Numerics` for math, custom namespaces, and is engine-agnostic.

### 2. Data Structures
- **Legacy:**
  - Uses Unity types for animation data.
  - `AnimBone` and `RPSAnimData` are Unity-centric.
  - Fields like `bones`, `rotBonesAnimated`, `packedRotation`, etc., are public and Unity-typed.
- **CwLibNet_4_HUB:**
  - Uses nullable `Vector4?[]` for animation data.
  - `AnimBone` and `RpsAnimData` are more generic.
  - More explicit about nullability and initialization.

### 3. Serialization
- **Legacy:**
  - Uses `Serializer.Serialize(ref field)` and returns `SERIALIZER_RESULT`.
  - Handles both reading and writing in a single method, with explicit field-by-field serialization.
  - Manual handling of binary data and endianness.
- **CwLibNet_4_HUB:**
  - Uses a versioned, revision-aware `Serializer` with methods like `I32`, `I16`, `V4`, etc.
  - Serialization is modular, with clear separation of reading and writing logic.
  - Handles new/old fields based on version.

### 4. Methods & Logic
- **Legacy:**
  - Contains Unity-specific helper methods for reading binary data, half-floats, etc.
  - Animation frame unpacking and blending is tightly coupled to Unity types and logic.
  - Caches frame data for performance.
- **CwLibNet_4_HUB:**
  - Animation frame unpacking and blending is more generic and modular.
  - Uses more idiomatic C# for caching and data access.
  - Provides additional utility methods for querying animation state and frames.

### 5. Error Handling & Return Types
- **Legacy:**
  - Uses `SERIALIZER_RESULT` for error/status reporting.
  - Manual error handling.
- **CwLibNet_4_HUB:**
  - Uses exceptions and null checks for error handling.
  - No explicit status codes.

### 6. Extensibility & Maintainability
- **Legacy:**
  - Tightly coupled to Unity and game runtime.
  - Harder to extend for non-Unity use cases.
- **CwLibNet_4_HUB:**
  - Engine-agnostic, modular, and easier to maintain or extend.
  - Designed for use as a general-purpose library.

---

## Detailed Comparison: Serializer.cs (Legacy vs. CwLibNet_4_HUB)

### 1. Classes & Structure
- **Legacy:**
  - Static `Serializer` class with nested static classes and enums.
  - Uses a global static `SerializerInfo` object for state.
  - IO handled with `BinaryReader`/`BinaryWriter`.
  - Error handling via `SERIALIZER_RESULT` enum.
  - Heavy Unity integration (types and methods).
  - Uses reflection for text resource reading.
- **CwLibNet_4_HUB:**
  - Likely instance-based `Serializer` class (not static/global).
  - Explicit versioning and revision handling.
  - Error handling via exceptions/null checks.
  - IO handled with custom stream classes.
  - No Unity dependency; uses .NET types.
  - Serialization via explicit methods (e.g., `I32`, `F32`, `V4`, `Array`).
  - No reflection-based text serialization.

### 2. Enums
- **Legacy:**
  - `SERIALIZER_RESULT` for error/status codes.
  - Compression flags as static classes.
  - Branch IDs as static classes.
- **CwLibNet_4_HUB:**
  - No `SERIALIZER_RESULT`; uses exceptions.
  - Compression/branch IDs likely as enums or constants.
  - Uses enums for resource/serialization types.

### 3. Migration Notes
- Remove `SERIALIZER_RESULT` and refactor to use exceptions.
- Refactor away from static/global state to instance-based serializers.
- Replace Unity types with .NET types.
- Update serialization methods to use explicit serializer instance methods.
- Remove reflection-based text serialization in favor of type-safe approaches.
- Use enums or bitfields for compression flags.

---

## Serializer Patterns: CwLibNet vs. Legacy (RMesh.cs)

### CwLibNet (Structs/Resources)
- Implements `ISerializable` interface with a `Serialize(Serializer serializer)` method.
- Uses an instance of `Serializer` (not static/global state).
- Serialization methods are void, with errors handled via exceptions.
- Field serialization uses methods like `serializer.S32`, `serializer.F32`, `serializer.Resource`, etc.
- Handles versioning with explicit checks (e.g., `serializer.GetRevision().GetSubVersion()`).
- Arrays and optionals are handled with null checks and length serialization.
- Pattern is modular, type-safe, and extensible.

### Legacy (RMesh.cs)
- No interface; serialization is via a `Serialize()` method returning a `SERIALIZER_RESULT` enum.
- Uses static `Serializer.Serialize(ref field)` methods and global state.
- Error handling via return codes, not exceptions.
- Versioning via `Serializer.SerializerInfo.DataVersion`.
- Unity types and attributes are used throughout.
- Arrays are handled with manual length and element serialization.
- Less modular and less type-safe than CwLibNet.

### Key Differences
- CwLibNet uses interface-based, instance-driven, exception-based, and modular serialization.
- Legacy uses static/global, return-code-based, and Unity-centric serialization.
- CwLibNet’s pattern is more extensible, automatable, and maintainable.

---

## Additional Analysis Areas

### 1. Field Types & Naming Conventions
- Compare field types (e.g., float[] vs. Vector2/3/4, int vs. short, etc.) between CwLibNet_4_HUB and Craftworld HUB.
- Check for naming convention mismatches (camelCase vs. PascalCase, etc.).
- Note any fields present in one version but not the other.

### 2. Enum Differences
- Compare enums (names, values, usage) between both codebases.
- Document any enums that need to be mapped or aliased.

### 3. Array & Collection Handling
- Analyze how arrays, lists, and dictionaries are serialized/deserialized.
- Note any differences in null handling, length prefixing, or type safety.

### 4. Custom Types & Structs
- List custom types (e.g., ResourceReference, AnimBone, etc.) and compare their structure and serialization.
- Document any type wrapping/unwrapping or conversion needed.

### 5. Versioning & Compatibility
- Compare how versioning is handled in both serializers.
- Note any fields or logic that are version-dependent.

### 6. Error Handling & Logging
- Compare error handling (exceptions vs. return codes).
- Note any logging or debug output differences.

### 7. Unity/Engine-Specific Logic
- Identify any Unity-specific logic that must be retained, removed, or replaced.
- Document any dependencies on UnityEditor, UnityEngine, etc.

### 8. Serialization Entry Points
- Compare how serialization is triggered (e.g., via methods, attributes, or reflection).
- Note any differences in how resources are loaded/saved.

### 9. Performance Considerations
- Document any performance-critical code (e.g., large arrays, memory streams, etc.).
- Note any optimizations or bottlenecks.

### 10. Testing & Validation
- Compare test coverage and strategies for serialization.
- Note any test cases or edge cases that must be preserved.

---

*Proceeding to analyze each area in detail...*

## Detailed Analysis Results

### Field Types and Naming Conventions

**CwLibNet_4_HUB Patterns:**
- **Field Naming**: PascalCase for all public fields (e.g., `Id`, `Root`, `Location`, `AuthorName`)
- **Constants**: PascalCase with descriptive names (e.g., `BaseAllocationSize`, `MaxBoneNameLength`)
- **Method Names**: PascalCase (e.g., `Serialize`, `GetAllocatedSize`, `GetName`)
- **Null Safety**: Extensive use of nullable types (`string?`, `Label[]?`, `Vector4?`)
- **Collection Types**: Arrays with nullable notation (`Label[]?`, `Collectabubble[]?`)

**Primitive Types:**
- Standard C# types: `int`, `byte`, `short`, `float`, `bool`, `string`
- System.Numerics: `Vector4`, `Matrix4x4` (for 3D math)
- Custom types: `GUID`, `ResourceDescriptor`, `SlotID`, `NetworkOnlineId`

**Legacy Craftworld Patterns (from previous analysis):**
- **Field Naming**: camelCase for private fields, PascalCase for properties
- **Method Names**: PascalCase
- **Arrays**: Standard C# arrays without nullable annotations
- **Unity Types**: Vector3, Quaternion, Transform dependencies

### Enum Handling

**CwLibNet_4_HUB Patterns:**
```csharp
public enum LevelType
{
    MAIN_PATH,      // 0 - implicit
    MINI_LEVEL,     // 1 - implicit
    MINI_GAME,      // 2 - implicit
    // ...
}

// Complex enum wrapper pattern:
public sealed class LevelBody
{
    private readonly LevelType value;
    
    public static LevelBody FromValue(int value)
    {
        return Enum.IsDefined(typeof(LevelType), value) 
            ? new LevelBody(value) 
            : new LevelBody((int)LevelType.MAIN_PATH);
    }
}
```

**Enum Serialization:**
```csharp
DeveloperLevelType = serializer.Enum32(DeveloperLevelType);
GameProgressionStatus = serializer.Enum32(GameProgressionStatus);
```

### Array and Collection Handling

**CwLibNet_4_HUB Patterns:**
```csharp
Labels = serializer.Array<Label>(Labels);
ShapeVerts = serializer.Array(ShapeVerts);
CollectabubblesRequired = new Collectabubble[]?
SubLevels = SlotID[]?
```

**Array Initialization:**
```csharp
if (descriptor.Labels != null)
{
    Labels = new Label[descriptor.Labels.Length];
    for (var i = 0; i < Labels.Length; i++)
    {
        Labels[i] = new Label((int) RTranslationTable.MakeLamsKeyID(descriptor.Labels[i]), i);
    }
}
```

### Custom Type Patterns

**GUID Implementation:**
```csharp
public readonly struct GUID : IEquatable<GUID>
{
    public readonly long Value;
    public GUID(long value) { Value = value; }
    public override string ToString() => "g" + Value;
    // Proper equality implementation
}
```

**Struct Inheritance:**
```csharp
public class Bone: AnimBone  // Inheritance for specialized structs
{
    public new const int BaseAllocationSize = AnimBone.BaseAllocationSize + 0x120;
    // Additional fields and overrides
}
```

### Versioning and Conditional Serialization

**Sophisticated Version Handling:**
```csharp
public void Serialize(Serializer serializer)
{
    var revision = serializer.GetRevision();
    var version = revision.GetVersion();
    var subVersion = revision.GetSubVersion();

    // Version-dependent field serialization
    if (subVersion >= (int)Revisions.ADVENTURE)
        Adventure = serializer.Resource(Adventure, ResourceType.Level, true);
    
    if (version >= 0x13b)
        AuthorName = serializer.Wstr(AuthorName);
        
    switch (version)
    {
        case >= 0x333:
            PlanetDecorations = serializer.Resource(PlanetDecorations, ResourceType.Plan, true);
            break;
        case < 0x188:
            serializer.U8(0); // Unknown/deprecated field
            break;
    }
}
```

### Serializer Method Patterns

**CwLibNet_4_HUB Serializer Methods:**
```csharp
// Primitive types
I32(int value)           // 32-bit integer
S32(int value)           // signed 32-bit
U8(int value)            // unsigned byte
F32(float value)         // 32-bit float
Bool(bool value)         // boolean
Wstr(string value)       // wide string
Str(string value)        // regular string

// Complex types
V4(Vector4? value)       // Vector4
M44(Matrix4x4? value)    // Matrix4x4
Guid(GUID value)         // GUID
Enum32<T>(T value)       // 32-bit enum
Resource(ResourceDescriptor, ResourceType, bool) // Resource reference
Struct<T>(T value)       // Generic struct
Array<T>(T[] value)      // Generic array
```

**Legacy Craftworld Patterns (inferred):**
- Simpler method names (readInt, writeInt, etc.)
- Less type safety
- Manual array handling
- Basic versioning

### Memory Management and Size Calculation

**CwLibNet_4_HUB Patterns:**
```csharp
public const int BaseAllocationSize = 0x40;  // Base size constants
public new const int BaseAllocationSize = AnimBone.BaseAllocationSize + 0x120;  // Inheritance

public int GetAllocatedSize()
{
    return BaseAllocationSize;  // Override for dynamic sizing
}
```

### Error Handling and Validation

**CwLibNet_4_HUB Patterns:**
```csharp
// Enum validation with fallbacks
public static LevelBody FromValue(int value)
{
    return Enum.IsDefined(typeof(LevelType), value) 
        ? new LevelBody(value) 
        : new LevelBody((int)LevelType.MAIN_PATH);
}

// Range clamping
MinPlayers = (byte)Math.Clamp(descriptor.Minplayers, 1, 4);
MaxPlayers = (byte)Math.Clamp(descriptor.Maxplayers, 1, 4);

// String length validation
if (name is { Length: >= MaxBoneNameLength })
    name = name[..MaxBoneNameLength];
```

### Key Differences Summary

| Aspect | CwLibNet_4_HUB | Legacy Craftworld |
|--------|----------------|-------------------|
| **Field Naming** | PascalCase consistently | Mixed camelCase/PascalCase |
| **Null Safety** | Extensive nullable types | No nullable annotations |
| **Enums** | Complex wrapper classes | Simple enum usage |
| **Versioning** | Sophisticated multi-level | Basic version checks |
| **Arrays** | Generic `Array<T>()` methods | Manual array handling |
| **Types** | System.Numerics, custom structs | Unity types, basic C# |
| **Error Handling** | Validation, clamping, fallbacks | Basic error handling |
| **Memory** | Size calculation constants | Manual size management |
| **Serializer** | Type-safe method names | Generic read/write methods |
