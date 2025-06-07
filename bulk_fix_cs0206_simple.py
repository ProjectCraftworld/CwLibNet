#!/usr/bin/env python3
"""
Bulk fix for CS0206 errors using pre-extracted error list
"""

import os
import re
from pathlib import Path

def fix_cs0206_in_file(file_path, line_numbers):
    """Fix CS0206 errors in a specific file."""
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
            line = lines[line_idx]
            
            print(f"Processing line {line_num}: {line.strip()}")
            
            # Skip if line doesn't contain ref/out with property access
            if not re.search(r'\b(ref|out)\s+[A-Za-z_]\w*', line):
                print(f"  Skipping - no ref/out pattern found")
                continue
            
            original_line = line
            
            # Simple: Serializer.Serialize(ref PropertyName);
            pattern1 = r'(\s*)([^.]*Serializer)\.Serialize\s*\(\s*(ref|out)\s+([A-Za-z_]\w*)\s*\)\s*;'
            if re.search(pattern1, line):
                new_line = re.sub(pattern1, r'\1var temp_\4 = \4;\n\1\2.Serialize(\3 temp_\4);\n\1\4 = temp_\4;', line)
                lines[line_idx] = new_line
                changes_made += 1
                print(f"  Fixed simple pattern: {original_line.strip()}")
                continue
            
            # With parameters: Serializer.Serialize(ref PropertyName, param1, param2, ...);
            pattern2 = r'(\s*)([^.]*Serializer)\.Serialize\s*\(\s*(ref|out)\s+([A-Za-z_]\w*)\s*,([^)]*)\)\s*;'
            if re.search(pattern2, line):
                new_line = re.sub(pattern2, r'\1var temp_\4 = \4;\n\1\2.Serialize(\3 temp_\4,\5);\n\1\4 = temp_\4;', line)
                lines[line_idx] = new_line
                changes_made += 1
                print(f"  Fixed parameterized pattern: {original_line.strip()}")
                continue
            
            print(f"  No pattern matched for: {line.strip()}")
        
        if changes_made > 0:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.writelines(lines)
            print(f"Made {changes_made} changes to {file_path}")
        
        return changes_made
        
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return 0

def main():
    # Read errors from file
    errors = []
    with open('/tmp/cs0206_errors.txt', 'r') as f:
        for line in f:
            match = re.match(r'([^(]+)\((\d+),\d+\): error CS0206', line.strip())
            if match:
                file_path = match.group(1)
                line_num = int(match.group(2))
                errors.append((file_path, line_num))
    
    print(f"Found {len(errors)} CS0206 errors")
    
    # Group errors by file
    file_errors = {}
    for file_path, line_num in errors:
        if file_path not in file_errors:
            file_errors[file_path] = []
        file_errors[file_path].append(line_num)
    
    total_fixed = 0
    for file_path, line_numbers in file_errors.items():
        print(f"\nProcessing {file_path} ({len(line_numbers)} errors)...")
        fixed = fix_cs0206_in_file(file_path, line_numbers)
        total_fixed += fixed
    
    print(f"\nTotal changes made: {total_fixed}")

if __name__ == "__main__":
    main()
