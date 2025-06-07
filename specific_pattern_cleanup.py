#!/usr/bin/env python3
import os
import re

def fix_specific_remaining_patterns():
    """Fix specific remaining serializer patterns found in the codebase"""
    
    root_dir = "/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB"
    files_changed = 0
    total_replacements = 0
    
    # Define specific patterns found in the codebase
    patterns = [
        # Array methods
        (r'(\w+)\s*=\s*serializer\.Arraylist<(\w+)>\((\w+)\)', r'\1 = Serializer.Serialize(ref \3)'),
        (r'(\w+)\s*=\s*serializer\.Boolarray\(([^)]+)\)', r'\1 = Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Intvector\(([^)]+)\)', r'\1 = Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Floatarray\(([^)]+)\)', r'\1 = Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Vectorarray\(([^)]+)\)', r'\1 = Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Bytearray\(([^)]+)\)', r'\1 = Serializer.Serialize(ref \2)'),
        
        # Struct methods
        (r'(\w+)\s*=\s*serializer\.Struct<(\w+)>\((\w+)\)', r'\1 = Serializer.Serialize(ref \3)'),
        (r'(\w+)\s*=\s*serializer\.Struct<(\w+)>\(null\)', r'\1 = Serializer.Serialize(ref \1)'),
        
        # Enum methods
        (r'(\w+)\s*=\s*serializer\.Enum32<(\w+)>\((\w+)\)', r'\1 = Serializer.Serialize(ref \3)'),
        
        # Vector and Matrix methods
        (r'(\w+)\s*=\s*\([^)]+\)\s*serializer\.V3\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        (r'(\w+)\s*=\s*serializer\.V4\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        (r'(\w+)\s*=\s*serializer\.M44\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        
        # Hash methods
        (r'serializer\.Sha1\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        
        # Pointer methods - these are more complex and might need special handling
        (r'var\s+(\w+)\s*=\s*serializer\.GetPointer<(\w+)>\(([^)]+)\)', r'var \1 = Serializer.GetPointer<\2>(\3)'),
        
        # Array assignments with indexing
        (r'(\w+)\[(\w+)\]\s*=\s*\([^)]+\)\s*serializer\.V3\(([^)]+)\)', r'Serializer.Serialize(ref \1[\2])'),
        
        # Collection initializations
        (r'(\w+)\s*=\s*\[serializer\.Struct<(\w+)>\(null\)\]', r'\1 = new \2[1]; Serializer.Serialize(ref \1[0])'),
        
        # Direct struct serialization calls
        (r'serializer\.Struct<(\w+)>\(this\)', r'Serializer.Serialize(ref this)'),
        (r'serializer\.Struct<(\w+)>\(null\)', r'Serializer.Serialize(ref default(\1))'),
    ]
    
    for root, dirs, files in os.walk(root_dir):
        for file in files:
            if file.endswith('.cs'):
                file_path = os.path.join(root, file)
                
                try:
                    with open(file_path, 'r', encoding='utf-8') as f:
                        content = f.read()
                    
                    original_content = content
                    file_replacements = 0
                    
                    # Apply all patterns
                    for pattern, replacement in patterns:
                        matches = re.findall(pattern, content)
                        if matches:
                            content = re.sub(pattern, replacement, content)
                            file_replacements += len(matches)
                    
                    # Only write if changes were made
                    if content != original_content:
                        with open(file_path, 'w', encoding='utf-8') as f:
                            f.write(content)
                        
                        files_changed += 1
                        total_replacements += file_replacements
                        print(f"Modified {file_path}: {file_replacements} replacements")
                
                except Exception as e:
                    print(f"Error processing {file_path}: {e}")
    
    print(f"\nSpecific pattern cleanup completed!")
    print(f"Files modified: {files_changed}")
    print(f"Total replacements: {total_replacements}")

if __name__ == "__main__":
    fix_specific_remaining_patterns()
