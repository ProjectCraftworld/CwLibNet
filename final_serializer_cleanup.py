#!/usr/bin/env python3
import os
import re

def clean_remaining_serializer_patterns():
    """Final cleanup for remaining serializer patterns"""
    
    root_dir = "/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB"
    files_changed = 0
    total_replacements = 0
    
    # Define patterns that need to be converted
    patterns = [
        # Array methods
        (r'(\w+)\s*=\s*serializer\.Shortarray\((\w+)\)', r'\1 = Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Intarray\((\w+)\)', r'\1 = Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Bytearray\((\w+)\)', r'\1 = Serializer.Serialize(ref \2)'),
        (r'(\w+)\s*=\s*serializer\.Floatarray\((\w+)\)', r'\1 = Serializer.Serialize(ref \2)'),
        
        # Basic type methods  
        (r'serializer\.I32\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.I16\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.I8\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.U32\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.U16\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.U8\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.F32\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.Bool\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        
        # Resource methods
        (r'serializer\.Reference\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        (r'serializer\.Resource\(([^,)]+),\s*([^,)]+)(?:,\s*([^)]+))?\)', r'Serializer.Serialize(ref \1, \2)'),
        (r'serializer\.Struct\(([^)]+)\)', r'Serializer.Serialize(ref \1)'),
        
        # Special method calls that need dependencies
        (r'serializer\.AddDependency\(([^)]+)\)', r'Serializer.AddDependency(\1)'),
        
        # Comments - these should stay as comments
        (r'//\s*Object _struct = serializer\.Struct\(([^)]+)\)', r'// Object _struct = Serializer.Serialize(ref \1)'),
        
        # Complex cases with casting
        (r'(\w+)\[(\w+)\]\s*=\s*\([^)]+\)\s*serializer\.I16\(([^)]+)\)', r'Serializer.Serialize(ref \1[\2])'),
        (r'(\w+)\[(\w+)\]\s*=\s*\([^)]+\)\s*serializer\.I32\(([^)]+)\)', r'Serializer.Serialize(ref \1[\2])'),
        
        # Variable assignments with serializer methods
        (r'var\s+(\w+)\s*=\s*serializer\.I32\(\)', r'var \1 = Serializer.GetInput().I32()'),
        (r'(\w+)\s*=\s*\([^)]+\)\s*serializer\.Serialize\(([^)]+)\)', r'\1 = Serializer.Serialize(ref \2)'),
        
        # Specific patterns found in the code
        (r'var\s+numItems\s*=\s*serializer\.I32\(questDescriptors.*?\);', 
         r'var numItems = Serializer.Serialize(ref questDescriptors != null ? questDescriptors.Count : 0);'),
        
        # GetType pattern
        (r'serializer\.GetType\(\)\.GetMethod\("Struct"\)\?\.MakeGenericMethod\(([^)]+)\)\.Invoke\(serializer,\s*null\)',
         r'Serializer.Serialize(ref \1)'),
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
    
    print(f"\nFinal cleanup completed!")
    print(f"Files modified: {files_changed}")
    print(f"Total replacements: {total_replacements}")

if __name__ == "__main__":
    clean_remaining_serializer_patterns()
