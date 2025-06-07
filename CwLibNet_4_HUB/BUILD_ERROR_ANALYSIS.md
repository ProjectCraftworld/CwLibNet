# Build Error Analysis - CwLibNet Migration

This document categorizes and analyzes the 359 build errors found in the CwLibNet project migration.

## Error Categories Summary

### 1. **ref/out Parameter Issues** (32+ errors)
- **CS1510**: A ref or out value must be an assignable variable
- **CS1605**: Cannot use 'this' as a ref or out value because it is read-only
- **Examples**:
  - `RPalette.cs(36,55)`: `ref` parameter with non-assignable variable
  - `RGfxMaterial.cs(259,34)`: Using `this` as ref in readonly context
  - `RPalette.cs(72,34)`: Same `this` as ref issue
  - Multiple similar issues in Resources/ files

### 2. **Type Conversion/Assignment Issues** (40+ errors)
- **CS0029**: Cannot implicitly convert type 'void' to expected type
- **CS0030**: Cannot convert type 'void' to expected type
- **Examples**:
  - `RLevel.cs(102,21)`: Converting void to `Sha1`
  - `RLevel.cs(136,24)`: Converting void to `byte[]`
  - `PlayedLevelData.cs`: Multiple void to short conversions

### 3. **Missing Context/Undefined Names** (25+ errors)
- **CS0103**: The name 'X' does not exist in the current context
- **Missing variables/properties**:
  - `Revisions` - Multiple files
  - `Branch` - RPlan.cs
  - `ResourceType` - Multiple files
  - `SerializationType` - Multiple files
  - `Matrix4x4` - Multiple files
  - `Part` - RPlan.cs
  - `temp_float_0` - PBody.cs

### 4. **Static vs Instance Method Issues** (15+ errors)
- **CS0120**: An object reference is required for non-static members
- **Examples**:
  - `RPlan.cs`: Calling `Serializer.ClearDependencies()` as static
  - `RPlan.cs`: Calling `Serializer.GetCompressionFlags()` as static
  - `RPlan.cs`: Calling `Serializer.Array<Thing>()` as static
  - `Serializer.cs`: Multiple static vs instance confusion

### 5. **Generic Type Constraint Failures** (50+ errors)
- **CS0315**: Type cannot be used as type parameter 'T' - no boxing conversion to ISerializable
- **CS0311**: Type cannot be used as type parameter 'T' - no implicit reference conversion
- **Affected types**:
  - Enums: `CellGcmPrimitive`, `HairMorph`, `SkeletonType`, `PlayMode`, `InventorySortMode`, `LevelType`, `GameProgressionStatus`
  - Value types: `uint`, `Matrix4x4`, `Vector4`, `GUID`
  - Object types

### 6. **Missing Method Arguments** (35+ errors)
- **CS7036**: There is no argument given that corresponds to required parameter
- **CS1503**: Argument type conversion issues
- **CS1501**: No overload for method takes X arguments
- **Pattern**: `Serializer.Serialize()` calls missing required parameters (`t`, `cp`, etc.)

## Priority Fixes

### **Priority 1: Critical Infrastructure**
1. **Fix Serializer class static/instance issues**
   - Many methods being called as static when they should be instance
   - Core serialization functionality broken

2. **Restore missing properties/variables**
   - `Revisions`, `Branch`, `ResourceType`, `SerializationType`
   - These appear to be migration artifacts

3. **Fix ref/out parameter usage**
   - Cannot use `this` as ref/out in readonly structs
   - Need to use temporary variables

### **Priority 2: Type System Issues**
1. **Generic constraints for Serializer.Serialize<T>()**
   - Many primitive types and enums don't implement ISerializable
   - Need specialized overloads or different approach

2. **Method signature mismatches**
   - Serializer.Serialize() calls with wrong argument counts/types
   - Appears to be from API changes during migration

### **Priority 3: Data Type Issues**
1. **void return type assignments**
   - Methods returning void being assigned to variables
   - Likely from incomplete migration of method calls

2. **Missing using statements**
   - `Matrix4x4` and other types not found
   - Need proper namespace imports

## Recommended Fix Strategy

### Phase 1: Infrastructure
1. Review and fix Serializer class method signatures
2. Restore missing static properties/constants
3. Add missing using statements

### Phase 2: Serialization
1. Fix generic type constraints in Serializer
2. Add proper overloads for primitive types
3. Fix ref/out parameter usage patterns

### Phase 3: Data Flow
1. Fix void return assignments
2. Correct method call signatures
3. Validate type conversions

### Phase 4: Validation
1. Incremental builds after each major fix
2. Test basic functionality
3. Address remaining edge cases

## Files Requiring Immediate Attention

### Core Infrastructure:
- `IO/Serializer/Serializer.cs` - Core serialization logic
- `Resources/RPlan.cs` - Missing critical properties
- `Resources/RSlotList.cs` - Missing properties

### High Error Count:
- `Structs/Things/Parts/PBody.cs` - Multiple undefined variables
- `Structs/Slot/Slot.cs` - Many serialization issues
- `Resources/RLevel.cs` - Type conversion issues

This analysis provides a roadmap for systematically addressing the build errors in order of impact and dependency.
