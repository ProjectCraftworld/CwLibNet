#!/usr/bin/env python3
"""
Fix CS0120 errors by replacing static calls with instance calls through GetCurrentSerializer()
"""

import re
import subprocess

def get_cs0120_errors():
    """Get CS0120 errors from build output."""
    result = subprocess.run(['dotnet', 'build'], cwd='/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB', capture_output=True, text=True)
    output = result.stdout + result.stderr
    
    errors = []
    for line in output.split('\n'):
        if 'error CS0120' in line:
            # Extract the method name from the error message
            match = re.search(r"'Serializer\.([^'(]+)", line)
            if match:
                method_name = match.group(1)
                # Extract file and line
                file_match = re.match(r'([^(]+)\((\d+),', line)
                if file_match:
                    file_path = file_match.group(1)
                    line_num = int(file_match.group(2))
                    errors.append((file_path, line_num, method_name))
    
    return errors

def fix_file(file_path, fixes):
    """Fix static calls in a file."""
    try:
        with open(file_path, 'r') as f:
            lines = f.readlines()
        
        # Sort by line number descending to avoid offset issues
        fixes = sorted(fixes, key=lambda x: x[0], reverse=True)
        
        changes = 0
        for line_num, method_name in fixes:
            line_idx = line_num - 1
            if line_idx < len(lines):
                line = lines[line_idx]
                # Replace Serializer.MethodName with Serializer.GetCurrentSerializer().MethodName
                pattern = rf'\bSerializer\.{re.escape(method_name)}\b'
                if re.search(pattern, line):
                    new_line = re.sub(pattern, f'Serializer.GetCurrentSerializer().{method_name}', line)
                    lines[line_idx] = new_line
                    changes += 1
                    print(f"Fixed {file_path}:{line_num} - {method_name}")
        
        if changes > 0:
            with open(file_path, 'w') as f:
                f.writelines(lines)
            print(f"Made {changes} changes to {file_path}")
        
        return changes
    except Exception as e:
        print(f"Error fixing {file_path}: {e}")
        return 0

def main():
    print("Finding CS0120 errors...")
    errors = get_cs0120_errors()
    print(f"Found {len(errors)} CS0120 errors")
    
    # Group by file
    file_fixes = {}
    for file_path, line_num, method_name in errors:
        if file_path not in file_fixes:
            file_fixes[file_path] = []
        file_fixes[file_path].append((line_num, method_name))
    
    total_fixed = 0
    for file_path, fixes in file_fixes.items():
        print(f"\nFixing {file_path}...")
        fixed = fix_file(file_path, fixes)
        total_fixed += fixed
    
    print(f"\nTotal fixes: {total_fixed}")
    
    # Check remaining errors
    remaining = get_cs0120_errors()
    print(f"Remaining CS0120 errors: {len(remaining)}")

if __name__ == "__main__":
    main()
