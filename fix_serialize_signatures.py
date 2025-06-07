#!/usr/bin/env python3
import os
import re

def fix_serialize_methods(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    changed = False
    
    # Fix Serialize() methods that should take Serializer parameter
    # Pattern: public override void Serialize()
    pattern1 = r'public override void Serialize\(\)'
    replacement1 = 'public override void Serialize(Serializer.Serializer serializer)'
    if re.search(pattern1, content):
        content = re.sub(pattern1, replacement1, content)
        changed = True
    
    # Pattern: public void Serialize()
    pattern2 = r'public void Serialize\(\)'
    replacement2 = 'public void Serialize(Serializer.Serializer serializer)'
    if re.search(pattern2, content):
        content = re.sub(pattern2, replacement2, content)
        changed = True
    
    # Pattern: void Serialize()
    pattern3 = r'(\s+)void Serialize\(\)'
    replacement3 = r'\1void Serialize(Serializer.Serializer serializer)'
    if re.search(pattern3, content):
        content = re.sub(pattern3, replacement3, content)
        changed = True
    
    if changed:
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)
        print(f"Fixed Serialize method signature in: {file_path}")
        return True
    
    return False

def scan_directory(directory):
    fixed_count = 0
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.cs'):
                file_path = os.path.join(root, file)
                if fix_serialize_methods(file_path):
                    fixed_count += 1
    return fixed_count

if __name__ == "__main__":
    directory = "/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB"
    fixed = scan_directory(directory)
    print(f"Fixed {fixed} files")
