#!/usr/bin/env python3

import os
import re
import argparse
from pathlib import Path

def migrate_serializer_patterns(content):
    """
    Migrate all serializer patterns to static Serializer calls
    """
    lines = content.split('\n')
    migrated_lines = []
    
    for line in lines:
        original_line = line
        
        # Skip lines that are already using static Serializer.
        if 'Serializer.Serialize(' in line or 'Serializer.GetRevision(' in line:
            migrated_lines.append(line)
            continue
            
        # Convert serializer.GetRevision() to Serializer.GetRevision()
        line = re.sub(r'\bserializer\.GetRevision\(\)', 'Serializer.GetRevision()', line)
        
        # Convert serializer.IsWriting() to Serializer.IsWriting()
        line = re.sub(r'\bserializer\.IsWriting\(\)', 'Serializer.IsWriting()', line)
        
        # Convert serializer.GetInput() to Serializer.GetInput()
        line = re.sub(r'\bserializer\.GetInput\(\)', 'Serializer.GetInput()', line)
        
        # Convert serializer.GetOutput() to Serializer.GetOutput()
        line = re.sub(r'\bserializer\.GetOutput\(\)', 'Serializer.GetOutput()', line)
        
        # Convert serializer array methods to Serializer.Serialize
        array_patterns = [
            (r'(\w+)\s*=\s*serializer\.Floatarray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.Bytearray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.Shortarray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.Intvector\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.Intarray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.Enumarray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.Arraylist\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.Vectorarray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.Array<([^>]+)>\((\w+)\)', r'Serializer.Serialize(ref \3)'),
        ]
        
        for pattern, replacement in array_patterns:
            line = re.sub(pattern, replacement, line)
        
        # Convert serializer single value methods to Serializer.Serialize
        single_value_patterns = [
            (r'(\w+)\s*=\s*serializer\.S32\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.I32\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.I8\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.U8\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.I16\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.U16\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.U32\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.I64\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.U64\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.F32\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.F64\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.Bool\((\w+)\)', r'Serializer.Serialize(ref \2)'),
            (r'(\w+)\s*=\s*serializer\.Intbool\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        ]
        
        for pattern, replacement in single_value_patterns:
            line = re.sub(pattern, replacement, line)
        
        # Convert enum serialization (with optional parameters)
        line = re.sub(r'(\w+)\s*=\s*serializer\.Enum8\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \2)', line)
        line = re.sub(r'(\w+)\s*=\s*serializer\.Enum32\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \2)', line)
        
        # Convert struct serialization
        line = re.sub(r'(\w+)\s*=\s*serializer\.Struct\((\w+)\)', r'Serializer.Serialize(ref \2)', line)
        
        # Convert V4 (Vector4) serialization
        line = re.sub(r'(\w+)\s*=\s*serializer\.V4\((\w+)\)', r'Serializer.Serialize(ref \2)', line)
        line = re.sub(r'(\w+)\s*=\s*serializer\.Matrix\((\w+)\)', r'Serializer.Serialize(ref \2)', line)
        line = re.sub(r'(\w+)\s*=\s*serializer\.M44\((\w+)\)(?:!\.Value)?', r'Serializer.Serialize(ref \2)', line)
        
        # Convert Resource serialization
        line = re.sub(r'(\w+)\s*=\s*serializer\.Resource\(([^)]+)\)', r'Serializer.Serialize(ref \1, \2)', line)
        
        # Convert string serialization
        line = re.sub(r'(\w+)\s*=\s*serializer\.Str\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \2)', line)
        line = re.sub(r'(\w+)\s*=\s*serializer\.Wstr\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \2)', line)
        
        # Convert reference serialization
        line = re.sub(r'(\w+)\s*=\s*serializer\.Reference(?:<[^>]+>)?\((\w+)\)', r'Serializer.Serialize(ref \2)', line)
        
        # Handle array index assignments
        line = re.sub(r'(\w+)\[(\w+)\]\s*=\s*serializer\.(\w+)\(([^)]+)\)', r'Serializer.Serialize(ref \1[\2])', line)
        
        # Convert direct calls without assignment (comprehensive list)
        line = re.sub(r'serializer\.I32\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.S32\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.S64\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.F32\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Bool\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.U8\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.I8\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.U16\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.u16\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.I16\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.U32\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.U64\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.I64\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.F64\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.V3\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.V4\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Matrix\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.M44\((\w+)\)(?:!\.Value)?(?:,\s*[^)]+)?', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Struct\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Enum8\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Enum32\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Intbool\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Guid\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Sha1\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Bytes\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        
        # Convert array methods
        line = re.sub(r'serializer\.Floatarray\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Bytearray\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Shortarray\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Intarray\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Longarray\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Boolarray\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Intvector\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Vectorarray\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Arraylist\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Thingarray\((\w+)(?:,\s*[^)]+)?\)', r'Serializer.Serialize(ref \1)', line)
        
        # Handle special methods that need to be kept as static method calls but changed to Serializer.
        line = re.sub(r'serializer\.Reference(<[^>]+>)?\(([^)]+)\)', r'Serializer.Reference\1(\2)', line)
        line = re.sub(r'serializer\.reference(<[^>]+>)?\(([^)]+)\)', r'Serializer.Reference\1(\2)', line)
        line = re.sub(r'serializer\.Array(<[^>]+>)?\(([^)]+)\)', r'Serializer.Array\1(\2)', line)
        line = re.sub(r'serializer\.Thing\(([^)]+)\)', r'Serializer.Reference(\1)', line)
        line = re.sub(r'serializer\.Resource\(([^)]+)\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Str\(([^)]+)\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.Wstr\(([^)]+)\)', r'Serializer.Serialize(ref \1)', line)
        line = re.sub(r'serializer\.AdventureCreatureReference\(([^)]+)\)', r'Serializer.Reference(\1)', line)
        
        # Handle utility methods
        line = re.sub(r'serializer\.GetBuffer\(\)', r'Serializer.GetBuffer()', line)
        line = re.sub(r'serializer\.GetDependencies\(\)', r'Serializer.GetDependencies()', line)
        line = re.sub(r'serializer\.GetThings\(\)', r'Serializer.GetThings()', line)
        line = re.sub(r'serializer\.GetType\(\)', r'Serializer.GetType()', line)
        line = re.sub(r'serializer\.GetRevision\(\)', r'Serializer.GetRevision()', line)
        line = re.sub(r'serializer\.GetCompressionFlags\(\)', r'Serializer.GetCompressionFlags()', line)
        line = re.sub(r'serializer\.GetPointer\(\)', r'Serializer.GetPointer()', line)
        line = re.sub(r'serializer\.GetNextReference\(\)', r'Serializer.GetNextReference()', line)
        line = re.sub(r'serializer\.SetPointer\(([^)]+)\)', r'Serializer.SetPointer(\1)', line)
        line = re.sub(r'serializer\.AddDependency\(([^)]+)\)', r'Serializer.AddDependency(\1)', line)
        line = re.sub(r'serializer\.ClearDependencies\(\)', r'Serializer.ClearDependencies()', line)
        line = re.sub(r'serializer\.Log\(([^)]+)\)', r'Serializer.Log(\1)', line)
        line = re.sub(r'serializer\.log\(([^)]+)\)', r'Serializer.Log(\1)', line)
        
        migrated_lines.append(line)
    
    return '\n'.join(migrated_lines)

def migrate_method_signature(content):
    """
    Remove Serializer parameter from Serialize method signatures
    """
    # Remove serializer parameter from Serialize method
    content = re.sub(
        r'public override void Serialize\(Serializer serializer\)',
        'public override void Serialize()',
        content
    )
    content = re.sub(
        r'public void Serialize\(Serializer serializer\)',
        'public void Serialize()',
        content
    )
    return content

def update_imports(content):
    """
    Update imports to use static Serializer
    """
    lines = content.split('\n')
    has_static_import = False
    has_old_import = False
    
    for i, line in enumerate(lines):
        if 'using static net.torutheredfox.craftworld.serialization.Serializer;' in line:
            has_static_import = True
        if 'using CwLibNet.IO.Serializer;' in line:
            has_old_import = True
            lines[i] = ''  # Remove old import
    
    # Add static import if not present
    if not has_static_import:
        # Find the namespace line and add import before it
        for i, line in enumerate(lines):
            if line.startswith('namespace '):
                lines.insert(i, 'using static net.torutheredfox.craftworld.serialization.Serializer;')
                lines.insert(i, '')  # Add blank line
                break
    
    return '\n'.join(lines)

def migrate_file(file_path):
    """
    Migrate a single C# file
    """
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Skip if file doesn't contain Serialize method
        if 'void Serialize(' not in content:
            return False
        
        original_content = content
        
        # Apply migrations
        content = update_imports(content)
        content = migrate_method_signature(content)
        content = migrate_serializer_patterns(content)
        
        # Only write if content changed
        if content != original_content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(content)
            return True
        
        return False
        
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return False

def main():
    parser = argparse.ArgumentParser(description='Migrate CwLibNet serialization to static pattern')
    parser.add_argument('directory', help='Directory to process')
    parser.add_argument('--dry-run', action='store_true', help='Show what would be changed without making changes')
    
    args = parser.parse_args()
    
    directory = Path(args.directory)
    if not directory.exists():
        print(f"Directory {directory} does not exist")
        return
    
    cs_files = list(directory.rglob('*.cs'))
    print(f"Found {len(cs_files)} C# files")
    
    changed_files = 0
    for file_path in cs_files:
        if args.dry_run:
            print(f"Would process: {file_path}")
        else:
            if migrate_file(file_path):
                changed_files += 1
                print(f"Migrated: {file_path}")
    
    if not args.dry_run:
        print(f"Migration complete. Changed {changed_files} files.")

if __name__ == '__main__':
    main()
