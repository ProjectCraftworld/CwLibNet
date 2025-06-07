#!/usr/bin/env python3
"""
Bulk fix for CS1023 errors: Embedded statement cannot be a declaration or labeled statement

This happens when we have if statements without braces followed by variable declarations.
"""

import os
import re
from pathlib import Path

def fix_cs1023_in_file(file_path, line_numbers):
    """Fix CS1023 errors in a specific file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            lines = f.readlines()
        
        # Sort line numbers in descending order to avoid offset issues
        line_numbers = sorted(set(line_numbers), reverse=True)
        
        changes_made = 0
        
        for line_num in line_numbers:
            if line_num > len(lines):
                continue
                
            line_idx = line_num - 1  # Convert to 0-based index
            line = lines[line_idx].strip()
            
            print(f"Processing line {line_num}: {line}")
            
            # Check if this is a variable declaration after an if statement
            if line.startswith('var temp_'):
                # Look at the previous line to see if it's an if statement
                if line_idx > 0:
                    prev_line = lines[line_idx - 1].rstrip()
                    if re.match(r'\s*if\s*\([^)]+\)\s*$', prev_line):
                        # This is an if statement without braces, we need to add them
                        # Find the block of related lines (var, serialize, assignment)
                        indent = len(prev_line) - len(prev_line.lstrip())
                        
                        # Find the end of the block
                        block_end = line_idx
                        temp_var_name = re.search(r'var (temp_\w+)', line).group(1)
                        
                        # Look for the serialize call and assignment
                        for i in range(line_idx + 1, min(line_idx + 10, len(lines))):
                            next_line = lines[i].strip()
                            if f'Serializer.Serialize(ref {temp_var_name})' in next_line:
                                block_end = i
                            elif f'{temp_var_name.replace("temp_", "")} = {temp_var_name}' in next_line:
                                block_end = i
                                break
                        
                        # Add braces around the block
                        indent_str = ' ' * (indent + 4)
                        brace_indent = ' ' * indent
                        
                        # Add opening brace after if statement
                        lines[line_idx - 1] = prev_line + '\n' + brace_indent + '{\n'
                        
                        # Indent all lines in the block
                        for i in range(line_idx, block_end + 1):
                            if lines[i].strip():  # Only indent non-empty lines
                                lines[i] = indent_str + lines[i].lstrip()
                        
                        # Add closing brace after the block
                        lines.insert(block_end + 1, brace_indent + '}\n')
                        
                        changes_made += 1
                        print(f"  Added braces around if block starting at line {line_num}")
        
        if changes_made > 0:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.writelines(lines)
            print(f"Made {changes_made} changes to {file_path}")
        
        return changes_made
        
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return 0

def main():
    # Get CS1023 errors
    os.system("cd /home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB && dotnet build 2>&1 | grep 'error CS1023' > /tmp/cs1023_errors.txt")
    
    # Read errors from file
    errors = []
    try:
        with open('/tmp/cs1023_errors.txt', 'r') as f:
            for line in f:
                match = re.match(r'([^(]+)\((\d+),\d+\): error CS1023', line.strip())
                if match:
                    file_path = match.group(1)
                    line_num = int(match.group(2))
                    errors.append((file_path, line_num))
    except FileNotFoundError:
        print("No CS1023 errors found!")
        return
    
    print(f"Found {len(errors)} CS1023 errors")
    
    # Group errors by file
    file_errors = {}
    for file_path, line_num in errors:
        if file_path not in file_errors:
            file_errors[file_path] = []
        file_errors[file_path].append(line_num)
    
    total_fixed = 0
    for file_path, line_numbers in file_errors.items():
        print(f"\nProcessing {file_path} ({len(line_numbers)} errors)...")
        fixed = fix_cs1023_in_file(file_path, line_numbers)
        total_fixed += fixed
    
    print(f"\nTotal changes made: {total_fixed}")

if __name__ == "__main__":
    main()
