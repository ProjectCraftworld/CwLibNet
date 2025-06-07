#!/usr/bin/env python3
import os
import re

def fix_imports(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Check if file uses SerializationData
    if 'SerializationData' not in content:
        return False
    
    # Check if CwLibNet.IO.Serializer import is already present
    if 'using CwLibNet.IO.Serializer;' in content:
        return False
    
    # Check if there are using statements
    using_pattern = r'^using [^;]+;'
    using_matches = re.findall(using_pattern, content, re.MULTILINE)
    
    if not using_matches:
        return False
    
    # Add the import after the last using statement
    last_using = using_matches[-1]
    new_import = 'using CwLibNet.IO.Serializer;'
    
    # Insert the import after the last using statement
    insertion_point = content.find(last_using) + len(last_using)
    new_content = content[:insertion_point] + '\n' + new_import + content[insertion_point:]
    
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(new_content)
    
    print(f"Fixed imports in: {file_path}")
    return True

def scan_directory(directory):
    fixed_count = 0
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.cs'):
                file_path = os.path.join(root, file)
                if fix_imports(file_path):
                    fixed_count += 1
    return fixed_count

if __name__ == "__main__":
    directory = "/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB"
    fixed = scan_directory(directory)
    print(f"Fixed {fixed} files")
