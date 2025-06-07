#!/usr/bin/env python3
import os
import re

def fix_serializer_imports(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    # Check if file has Serialize method or uses Serializer but doesn't import it
    if ('Serialize(' not in content and 'Serializer' not in content):
        return False
    
    # Check if CwLibNet.IO.Serializer import is already present
    if 'using CwLibNet.IO.Serializer;' in content:
        return False
    
    # Check if using static CwLibNet.IO.Serializer.Serializer is already present
    if 'using static CwLibNet.IO.Serializer.Serializer;' in content:
        return False
    
    # Check if there are using statements
    using_pattern = r'^using [^;]+;'
    using_matches = re.findall(using_pattern, content, re.MULTILINE)
    
    if not using_matches:
        return False
    
    # Find the best position to insert - after other CwLibNet imports but before static imports
    insert_after = None
    
    for i, using_stmt in enumerate(using_matches):
        if 'static' in using_stmt:
            break
        if 'CwLibNet' in using_stmt:
            insert_after = using_stmt
    
    if insert_after:
        target_using = insert_after
    else:
        target_using = using_matches[0]
    
    # Add the import after the target using statement
    new_import = 'using static CwLibNet.IO.Serializer.Serializer;'
    
    # Insert the import after the target using statement
    insertion_point = content.find(target_using) + len(target_using)
    new_content = content[:insertion_point] + '\n' + new_import + content[insertion_point:]
    
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(new_content)
    
    print(f"Fixed Serializer imports in: {file_path}")
    return True

def scan_directory(directory):
    fixed_count = 0
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.cs'):
                file_path = os.path.join(root, file)
                if fix_serializer_imports(file_path):
                    fixed_count += 1
    return fixed_count

if __name__ == "__main__":
    directory = "/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB"
    fixed = scan_directory(directory)
    print(f"Fixed {fixed} files")
