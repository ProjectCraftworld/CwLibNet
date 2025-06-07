#!/usr/bin/env python3
"""
Fix incorrect serialization patterns by converting temp variable workarounds 
back to proper serialization method calls.

Converts:
    var temp_Property = Property;
    Serializer.Serialize(ref temp_Property);
    Property = temp_Property;

To:
    Property = serializer.I32(Property);  // or appropriate method
"""

import os
import re
import subprocess
from pathlib import Path

def fix_serialization_patterns_in_file(file_path):
    """Fix serialization patterns in a specific file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        changes_made = 0
        
        # Pattern 1: Simple temp variable pattern for basic types
        # var temp_PropertyName = PropertyName;
        # Serializer.Serialize(ref temp_PropertyName);
        # PropertyName = temp_PropertyName;
        pattern1 = re.compile(
            r'(\s*)var temp_(\w+) = (\2);\s*\n'
            r'\s*Serializer\.Serialize\(ref temp_\2\);\s*\n'
            r'\s*\3 = temp_\2;',
            re.MULTILINE
        )
        
        def replace1(match):
            indent = match.group(1)
            property_name = match.group(2)
            return f'{indent}{property_name} = serializer.I32({property_name});'
        
        new_content = pattern1.sub(replace1, content)
        if new_content != content:
            changes_made += content.count('var temp_') - new_content.count('var temp_')
            content = new_content
        
        # Pattern 2: temp variable pattern with parameters
        # var temp_PropertyName = PropertyName;
        # Serializer.Serialize(ref temp_PropertyName, param1, param2, ...);
        # PropertyName = temp_PropertyName;
        pattern2 = re.compile(
            r'(\s*)var temp_(\w+) = (\2);\s*\n'
            r'\s*Serializer\.Serialize\(ref temp_\2,([^)]+)\);\s*\n'
            r'\s*\3 = temp_\2;',
            re.MULTILINE
        )
        
        def replace2(match):
            indent = match.group(1)
            property_name = match.group(2)
            params = match.group(4).strip()
            
            # Try to determine the correct serialization method based on parameters
            if 'ResourceType' in params:
                return f'{indent}{property_name} = serializer.Reference({property_name},{params});'
            elif 'true, false, false' in params:
                return f'{indent}{property_name} = serializer.Reference({property_name}, {params});'
            else:
                return f'{indent}{property_name} = serializer.I32({property_name});'
        
        new_content = pattern2.sub(replace2, content)
        if new_content != content:
            changes_made += content.count('var temp_') - new_content.count('var temp_')
            content = new_content
        
        # Pattern 3: Fix double temp variables (temp_temp_)
        pattern3 = re.compile(
            r'(\s*)var temp_temp_(\w+) = temp_(\2);',
            re.MULTILINE
        )
        
        def replace3(match):
            # Remove the double temp line entirely
            return ''
        
        content = pattern3.sub(replace3, content)
        
        # Pattern 4: Fix references to temp_temp_ variables
        content = re.sub(r'temp_temp_(\w+)', r'temp_\1', content)
        
        # Clean up any remaining orphaned temp variable declarations
        content = re.sub(r'\s*var temp_\w+ = \w+;\s*\n(?!\s*(?:Serializer|serializer))', '', content)
        
        if content != original_content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(content)
            print(f"Fixed serialization patterns in {file_path}")
            return True
        
        return False
        
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return False

def get_files_with_temp_variables():
    """Find all files that contain temp variable patterns."""
    try:
        result = subprocess.run(
            ['grep', '-r', '-l', 'var temp_', '.', '--include=*.cs'],
            cwd='/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB',
            capture_output=True,
            text=True
        )
        
        files = []
        for line in result.stdout.strip().split('\n'):
            if line and line.endswith('.cs'):
                files.append(os.path.join('/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB', line.lstrip('./')))
        
        return files
    except Exception as e:
        print(f"Error finding files: {e}")
        return []

def main():
    print("Finding files with temp variable patterns...")
    files = get_files_with_temp_variables()
    
    if not files:
        print("No files with temp variables found!")
        return
    
    print(f"Found {len(files)} files with temp variables")
    
    total_fixed = 0
    for file_path in files:
        print(f"Processing {file_path}...")
        if fix_serialization_patterns_in_file(file_path):
            total_fixed += 1
    
    print(f"\nFixed serialization patterns in {total_fixed} files")
    
    # Check the build status
    print("\nChecking build status...")
    try:
        result = subprocess.run(
            ['dotnet', 'build'], 
            cwd='/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB',
            capture_output=True, 
            text=True
        )
        
        output = result.stdout + result.stderr
        error_lines = [line for line in output.split('\n') if 'error CS' in line]
        print(f"Remaining errors: {len(error_lines)}")
        
        # Show error type counts
        error_types = {}
        for line in error_lines:
            match = re.search(r'error (CS\d+)', line)
            if match:
                error_type = match.group(1)
                error_types[error_type] = error_types.get(error_type, 0) + 1
        
        if error_types:
            print("Error type counts:")
            for error_type, count in sorted(error_types.items(), key=lambda x: x[1], reverse=True)[:10]:
                print(f"  {error_type}: {count}")
        
    except Exception as e:
        print(f"Error checking build status: {e}")

if __name__ == "__main__":
    main()
