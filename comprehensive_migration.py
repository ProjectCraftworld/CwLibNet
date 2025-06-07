#!/usr/bin/env python3

import os
import re
import sys

def comprehensive_migration(content):
    """
    Converts remaining serializer patterns to static Serializer calls
    """
    original_content = content
    
    # Fix Matrix4x4 serialization patterns
    content = re.sub(
        r'(\w+)\s*=\s*serializer\.M44\(\w+\)!\.Value;',
        r'Serializer.Serialize(ref \1);',
        content
    )
    
    # Fix Resource patterns
    content = re.sub(
        r'(\w+)\s*=\s*serializer\.Resource\((\w+),\s*([^)]+)\);',
        r'Serializer.Serialize(ref \1, \3);',
        content
    )
    
    # Fix GetInput().I32() patterns (need to handle these carefully)
    content = re.sub(
        r'Serializer\.GetInput\(\)\.I32\(\)',
        r'Serializer.GetInput().I32()',  # Keep for now, these are internal calls
        content
    )
    
    # Fix GetOutput().I32() patterns 
    content = re.sub(
        r'Serializer\.GetOutput\(\)\.I32\(([^)]+)\)',
        r'Serializer.GetOutput().I32(\1)',  # Keep for now, these are internal calls
        content
    )
    
    # Fix stream.Method() patterns to Serializer calls where appropriate
    # For array length serialization patterns
    content = re.sub(
        r'if\s*\(\s*!Serializer\.IsWriting\(\)\s*\)\s*\n\s*(\w+)\s*=\s*new\s+([^[]+)\[Serializer\.GetInput\(\)\.I32\(\)\];\s*\n\s*else\s+Serializer\.GetOutput\(\)\.I32\(([^)]+)\);',
        r'Serializer.Serialize(ref \1);',
        content,
        flags=re.MULTILINE
    )
    
    # Fix simple stream patterns
    content = re.sub(
        r'stream\.I32\(\)',
        r'Serializer.Serialize(ref temp_i32)',  # This will need proper handling
        content
    )
    
    content = re.sub(
        r'stream\.Bytes\(([^)]+)\)',
        r'Serializer.Serialize(ref \1)',
        content
    )
    
    # Fix remaining assignment patterns that weren't caught
    content = re.sub(
        r'(\w+)\s*=\s*Serializer\.Serialize\(ref\s+(\w+)\);',
        r'Serializer.Serialize(ref \1);',
        content
    )
    
    return content

def process_file(file_path):
    """Process a single file"""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        
        # Apply comprehensive migration
        content = comprehensive_migration(content)
        
        if content != original_content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(content)
            return True
        return False
        
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return False

def main():
    directory = "CwLibNet_4_HUB"
    
    if not os.path.exists(directory):
        print(f"Directory {directory} not found!")
        return
    
    modified_files = []
    
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith('.cs'):
                file_path = os.path.join(root, file)
                if process_file(file_path):
                    modified_files.append(file_path)
    
    print(f"Comprehensive migration completed!")
    print(f"Modified {len(modified_files)} files")
    
    if modified_files:
        print("\nModified files:")
        for file_path in modified_files:
            print(f"  - {file_path}")

if __name__ == "__main__":
    main()
