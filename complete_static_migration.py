#!/usr/bin/env python3
"""
Complete migration script to convert CwLibNet_4_HUB to use Toolkit's static Serializer API.
This script handles all serialization patterns and converts them to the correct static calls.
"""

import os
import re
import argparse
from pathlib import Path

def find_cs_files(directory):
    """Find all C# files in the directory."""
    cs_files = []
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.cs'):
                cs_files.append(os.path.join(root, file))
    return cs_files

def migrate_file(file_path):
    """Migrate a single C# file to use static serializer calls."""
    print(f"Processing: {file_path}")
    
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    original_content = content
    changes_made = 0
    
    # 1. Change method signature from Serialize(Serializer serializer) to Serialize()
    if re.search(r'public\s+(?:override\s+)?void\s+Serialize\s*\(\s*Serializer\s+\w+\s*\)', content):
        content = re.sub(
            r'public\s+((?:override\s+)?void\s+Serialize)\s*\(\s*Serializer\s+\w+\s*\)',
            r'public \1()',
            content
        )
        changes_made += 1
        print(f"  - Updated Serialize method signature")

    # 2. Convert instance-based calls to static calls
    
    # Basic types (these should already be static, but check for any remaining instance calls)
    patterns = [
        # Integer types
        (r'(\w+)\s*=\s*serializer\.I32\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.I16\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.I8\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.U32\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.U16\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.U8\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.I64\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.U64\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        
        # Float types
        (r'(\w+)\s*=\s*serializer\.F32\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.F16\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        
        # Boolean
        (r'(\w+)\s*=\s*serializer\.Bool\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Intbool\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        
        # String types
        (r'(\w+)\s*=\s*serializer\.Str\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Wstr\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Str\((\w+),\s*(\w+)\)', r'Serializer.Serialize(ref \2)'),  # Fixed length strings
        
        # Array types - these are the key conversions
        (r'(\w+)\s*=\s*serializer\.Bytearray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Intarray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Shortarray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Longarray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Floatarray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Vectorarray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Boolarray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        
        # Vector types
        (r'(\w+)\s*=\s*serializer\.V2\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.V3\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.V4\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        
        # Matrix types
        (r'(\w+)\s*=\s*serializer\.M44\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        
        # Direct method calls without assignment
        (r'serializer\.I32\((\w+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.I16\((\w+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.I8\((\w+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.U32\((\w+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.U16\((\w+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.U8\((\w+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.F32\((\w+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.Bool\((\w+)\)', r'Serializer.Serialize(ref \1)'),
    ]
    
    for pattern, replacement in patterns:
        before_count = len(re.findall(pattern, content))
        content = re.sub(pattern, replacement, content)
        after_count = before_count - len(re.findall(pattern, content))
        if after_count > 0:
            changes_made += after_count
            print(f"  - Replaced {after_count} instances of pattern: {pattern}")
    
    # 3. Remove serializer parameter references in method calls
    # Handle various serializer utility calls
    utility_patterns = [
        (r'serializer\.GetRevision\(\)', r'Serializer.GetRevision()'),
        (r'serializer\.IsWriting\(\)', r'Serializer.IsWriting()'),
        (r'serializer\.GetVersion\(\)', r'Serializer.GetVersion()'),
        (r'serializer\.GetInput\(\)', r'Serializer.GetInput()'),
        (r'serializer\.GetOutput\(\)', r'Serializer.GetOutput()'),
    ]
    
    for pattern, replacement in utility_patterns:
        before_count = len(re.findall(pattern, content))
        content = re.sub(pattern, replacement, content)
        after_count = before_count - len(re.findall(pattern, content))
        if after_count > 0:
            changes_made += after_count
            print(f"  - Replaced {after_count} utility calls: {pattern}")
    
    # 4. Handle complex array serialization patterns
    # Look for patterns like: thing?.Serialize() and convert to Serializer.Serialize(ref thing)
    complex_patterns = [
        # Object serialization
        (r'(\w+)\.Serialize\(\)', r'Serializer.Serialize(ref \1)'),
        (r'(\w+)\?\.Serialize\(\)', r'if (\1 != null) Serializer.Serialize(ref \1)'),
        
        # Array/List serialization with complex calls
        (r'(\w+)\s*=\s*serializer\.Array\((\w+),\s*false\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Array\((\w+),\s*true\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Arraylist\((\w+),\s*false\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Arraylist\((\w+),\s*true\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Thingarray\((\w+)\)', r'Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Thinglist\((\w+)\)', r'Serializer.Serialize(ref \2)'),
    ]
    
    for pattern, replacement in complex_patterns:
        before_count = len(re.findall(pattern, content))
        content = re.sub(pattern, replacement, content)
        after_count = before_count - len(re.findall(pattern, content))
        if after_count > 0:
            changes_made += after_count
            print(f"  - Replaced {after_count} complex patterns: {pattern}")
    
    # 5. Handle cases where we need to add "using static" directive
    if 'Serializer.Serialize(' in content and 'using static' not in content:
        # Add the using static directive at the top
        using_pattern = r'(using\s+[^;]+;\s*)+(\s*namespace)'
        if re.search(using_pattern, content):
            content = re.sub(
                using_pattern,
                r'\1using static net.torutheredfox.craftworld.serialization.Serializer;\n\2',
                content,
                count=1
            )
            changes_made += 1
            print(f"  - Added using static directive")
    
    # Write back if changes were made
    if changes_made > 0 and content != original_content:
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)
        print(f"  → {changes_made} changes applied to {file_path}")
        return True
    else:
        print(f"  → No changes needed for {file_path}")
        return False

def main():
    parser = argparse.ArgumentParser(description='Migrate CwLibNet_4_HUB to static Serializer API')
    parser.add_argument('directory', help='Directory to process')
    parser.add_argument('--dry-run', action='store_true', help='Show what would be changed without making changes')
    
    args = parser.parse_args()
    
    if not os.path.exists(args.directory):
        print(f"Error: Directory {args.directory} does not exist")
        return 1
    
    cs_files = find_cs_files(args.directory)
    print(f"Found {len(cs_files)} C# files to process")
    
    modified_files = 0
    for file_path in cs_files:
        if not args.dry_run:
            if migrate_file(file_path):
                modified_files += 1
        else:
            print(f"Would process: {file_path}")
    
    if not args.dry_run:
        print(f"\nMigration complete. Modified {modified_files} files.")
    else:
        print(f"\nDry run complete. Would modify files in {args.directory}")
    
    return 0

if __name__ == '__main__':
    exit(main())
