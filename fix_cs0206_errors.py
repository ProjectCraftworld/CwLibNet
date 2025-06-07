#!/usr/bin/env python3

import subprocess
import re
import os

def get_cs0206_errors():
    """Get all CS0206 errors from build output"""
    result = subprocess.run(['dotnet', 'build'], capture_output=True, text=True, cwd='/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB')
    errors = []
    for line in result.stderr.split('\n'):
        if 'error CS0206' in line:
            match = re.search(r'(.+?)\((\d+),\d+\):', line)
            if match:
                file_path = match.group(1)
                line_num = int(match.group(2))
                errors.append((file_path, line_num))
    return errors

def fix_property_serialization(file_path, line_num):
    """Fix a specific CS0206 error by replacing property serialization with temp variable"""
    try:
        with open(file_path, 'r') as f:
            lines = f.readlines()
        
        if line_num > len(lines):
            return False
            
        original_line = lines[line_num - 1]
        
        # Pattern: Serializer.Serialize(ref PropertyName)
        # or: Serializer.Serialize(ref PropertyName, ...)
        pattern = r'(\s*)Serializer\.Serialize\(ref\s+([A-Za-z_][A-Za-z0-9_]*)(.*?)\);?'
        match = re.match(pattern, original_line.strip())
        
        if match:
            indent = match.group(1)
            if not indent:
                # Get indentation from the original line
                indent = original_line[:len(original_line) - len(original_line.lstrip())]
            
            property_name = match.group(2)
            args = match.group(3)
            
            # Create temp variable name
            temp_var_name = f"temp{property_name}"
            
            # Create replacement lines
            replacement = [
                f"{indent}var {temp_var_name} = {property_name};\n",
                f"{indent}Serializer.Serialize(ref {temp_var_name}{args});\n",
                f"{indent}{property_name} = {temp_var_name};\n"
            ]
            
            # Replace the line
            lines[line_num - 1:line_num] = replacement
            
            with open(file_path, 'w') as f:
                f.writelines(lines)
            
            print(f"Fixed CS0206 in {file_path}:{line_num} - {property_name}")
            return True
    
    except Exception as e:
        print(f"Error fixing {file_path}:{line_num} - {e}")
        return False
    
    return False

def main():
    print("Fixing CS0206 errors...")
    
    errors = get_cs0206_errors()
    print(f"Found {len(errors)} CS0206 errors")
    
    # Process unique file/line combinations
    unique_errors = list(set(errors))
    
    fixed_count = 0
    for file_path, line_num in unique_errors:
        if fix_property_serialization(file_path, line_num):
            fixed_count += 1
    
    print(f"Fixed {fixed_count} CS0206 errors")

if __name__ == "__main__":
    main()
