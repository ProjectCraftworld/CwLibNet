#!/usr/bin/env python3

import os
import re
import sys

def fix_serializer_parameter_in_file(file_path):
    """Fix Serializer parameter type references in method signatures"""
    
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
    except UnicodeDecodeError:
        with open(file_path, 'r', encoding='utf-8-sig') as f:
            content = f.read()
    
    original_content = content
    
    # If the file has 'using static CwLibNet.IO.Serializer.Serializer;'
    # We need to use the fully qualified name CwLibNet.IO.Serializer.Serializer
    # instead of just Serializer.Serializer
    
    has_static_using = 'using static CwLibNet.IO.Serializer.Serializer;' in content
    
    if has_static_using:
        # Replace Serializer.Serializer with the full qualified name
        content = re.sub(
            r'\bSerialize\s*\(\s*Serializer\.Serializer\s+([^)]+)\)',
            r'Serialize(CwLibNet.IO.Serializer.Serializer \1)',
            content
        )
    
    # Also fix any void Serialize(Serializer serializer) methods to use the correct type
    content = re.sub(
        r'void\s+Serialize\s*\(\s*Serializer\s+([^)]+)\)',
        r'void Serialize(CwLibNet.IO.Serializer.Serializer \1)',
        content
    )
    
    # Fix override void Serialize(Serializer serializer) methods 
    content = re.sub(
        r'override\s+void\s+Serialize\s*\(\s*Serializer\s+([^)]+)\)',
        r'override void Serialize(CwLibNet.IO.Serializer.Serializer \1)',
        content
    )
    
    if content != original_content:
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)
        print(f"Fixed serializer parameter types in: {file_path}")
        return True
    
    return False

def main():
    project_dir = "/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB"
    
    # Find all C# files
    cs_files = []
    for root, dirs, files in os.walk(project_dir):
        for file in files:
            if file.endswith('.cs'):
                cs_files.append(os.path.join(root, file))
    
    total_files_fixed = 0
    
    for file_path in cs_files:
        if fix_serializer_parameter_in_file(file_path):
            total_files_fixed += 1
    
    print(f"\nTotal files fixed: {total_files_fixed}")

if __name__ == "__main__":
    main()
