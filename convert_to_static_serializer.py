#!/usr/bin/env python3
"""
Migration script to convert CwLibNet_4_HUB to use static Serializer.Serialize calls.

This script converts from:
    field = serializer.S32(field);
    field = serializer.I8(field);
    field = serializer.F32(field);
    etc.

To:
    Serializer.Serialize(ref field);

And updates method signatures and using statements accordingly.
"""

import os
import re
import sys
from pathlib import Path

def convert_file_to_static_serializer(file_path):
    """Convert a single C# file to use static Serializer.Serialize calls."""
    
    print(f"Processing: {file_path}")
    
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
    except Exception as e:
        print(f"Error reading {file_path}: {e}")
        return False
    
    original_content = content
    
    # 1. Update using statements - remove old serializer imports and add static import
    content = re.sub(r'using CwLibNet\.IO\.Serializer;\s*\n?', '', content)
    
    # Add static using if not present
    if 'using static net.torutheredfox.craftworld.serialization.Serializer;' not in content:
        # Find the last using statement and add after it
        using_pattern = r'(using [^;]+;)(?=\s*\nnamespace|\s*$)'
        if re.search(using_pattern, content):
            content = re.sub(using_pattern, r'\1\nusing static net.torutheredfox.craftworld.serialization.Serializer;', content, 1)
        else:
            # If no using statements, add at the beginning
            content = 'using static net.torutheredfox.craftworld.serialization.Serializer;\n' + content
    
    # 2. Update method signature from Serialize(Serializer serializer) to Serialize()
    content = re.sub(
        r'public\s+void\s+Serialize\s*\(\s*Serializer\s+serializer\s*\)',
        'public void Serialize()',
        content
    )
    
    # 3. Convert serializer method calls to static Serializer.Serialize calls
    
    # Pattern for: field = serializer.Method(field);
    patterns = [
        (r'(\w+)\s*=\s*serializer\.S32\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+)\s*=\s*serializer\.I32\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+)\s*=\s*serializer\.I8\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+)\s*=\s*serializer\.U8\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+)\s*=\s*serializer\.I16\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+)\s*=\s*serializer\.U16\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+)\s*=\s*serializer\.U32\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+)\s*=\s*serializer\.I64\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+)\s*=\s*serializer\.U64\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+)\s*=\s*serializer\.F32\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+)\s*=\s*serializer\.F64\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+)\s*=\s*serializer\.Bool\(\1\);', r'Serializer.Serialize(ref \1);'),
    ]
    
    for pattern, replacement in patterns:
        content = re.sub(pattern, replacement, content)
    
    # 4. Handle array access patterns like: array[i] = serializer.Method(array[i]);
    array_patterns = [
        (r'(\w+\[\w+\])\s*=\s*serializer\.S32\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\[\w+\])\s*=\s*serializer\.I32\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\[\w+\])\s*=\s*serializer\.I8\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\[\w+\])\s*=\s*serializer\.U8\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\[\w+\])\s*=\s*serializer\.I16\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\[\w+\])\s*=\s*serializer\.U16\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\[\w+\])\s*=\s*serializer\.U32\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\[\w+\])\s*=\s*serializer\.I64\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\[\w+\])\s*=\s*serializer\.U64\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\[\w+\])\s*=\s*serializer\.F32\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\[\w+\])\s*=\s*serializer\.F64\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\[\w+\])\s*=\s*serializer\.Bool\(\1\);', r'Serializer.Serialize(ref \1);'),
    ]
    
    for pattern, replacement in array_patterns:
        content = re.sub(pattern, replacement, content)
    
    # 5. Handle property access patterns like: obj.field = serializer.Method(obj.field);
    property_patterns = [
        (r'(\w+\.\w+)\s*=\s*serializer\.S32\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\.\w+)\s*=\s*serializer\.I32\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\.\w+)\s*=\s*serializer\.I8\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\.\w+)\s*=\s*serializer\.U8\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\.\w+)\s*=\s*serializer\.F32\(\1\);', r'Serializer.Serialize(ref \1);'),
        (r'(\w+\.\w+)\s*=\s*serializer\.Bool\(\1\);', r'Serializer.Serialize(ref \1);'),
    ]
    
    for pattern, replacement in property_patterns:
        content = re.sub(pattern, replacement, content)
    
    # 6. Convert other serializer method calls that don't follow the assignment pattern
    # Like: serializer.Array(someArray);
    content = re.sub(r'serializer\.Array\(([^)]+)\);', r'Serializer.Serialize(ref \1);', content)
    content = re.sub(r'serializer\.String\(([^)]+)\);', r'Serializer.Serialize(ref \1);', content)
    
    # Check if any changes were made
    if content == original_content:
        print(f"  No changes needed in {file_path}")
        return True
    
    # Write the modified content back
    try:
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)
        print(f"  ✓ Converted {file_path}")
        return True
    except Exception as e:
        print(f"  ✗ Error writing {file_path}: {e}")
        return False

def find_cs_files(directory):
    """Find all C# files in the given directory recursively."""
    cs_files = []
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.cs'):
                cs_files.append(os.path.join(root, file))
    return cs_files

def main():
    if len(sys.argv) < 2:
        print("Usage: python convert_to_static_serializer.py <directory> [--dry-run]")
        print("Example: python convert_to_static_serializer.py CwLibNet_4_HUB")
        sys.exit(1)
    
    directory = sys.argv[1]
    dry_run = '--dry-run' in sys.argv
    
    if not os.path.exists(directory):
        print(f"Error: Directory '{directory}' does not exist.")
        sys.exit(1)
    
    print(f"Converting C# files in '{directory}' to use static Serializer.Serialize calls...")
    if dry_run:
        print("DRY RUN MODE - No files will be modified")
    
    cs_files = find_cs_files(directory)
    
    if not cs_files:
        print(f"No C# files found in '{directory}'")
        return
    
    print(f"Found {len(cs_files)} C# files")
    
    success_count = 0
    
    for file_path in cs_files:
        if dry_run:
            print(f"Would process: {file_path}")
            success_count += 1
        else:
            if convert_file_to_static_serializer(file_path):
                success_count += 1
    
    print(f"\nCompleted: {success_count}/{len(cs_files)} files processed successfully")

if __name__ == "__main__":
    main()
