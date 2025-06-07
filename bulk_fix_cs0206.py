#!/usr/bin/env python3
"""
Bulk fix for CS0206 errors: A non ref-returning property or indexer may not be used as an out or ref value

This script finds serialization calls that use ref/out with properties and creates temporary variables.
"""

import os
import re
import subprocess
from pathlib import Path

def get_cs0206_errors():
    """Get all CS0206 errors from dotnet build output."""
    try:
        print("Running dotnet build...")
        result = subprocess.run(
            ['dotnet', 'build'], 
            cwd='/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB',
            capture_output=True, 
            text=True
        )
        
        errors = []
        # Check both stdout and stderr
        output = result.stdout + result.stderr
        print(f"Output length: {len(output)}")
        
        cs0206_lines = [line for line in output.split('\n') if 'error CS0206' in line]
        print(f"Found {len(cs0206_lines)} CS0206 lines")
        
        for line in cs0206_lines:
            # Extract file path and line number
            match = re.match(r'([^(]+)\((\d+),\d+\): error CS0206', line)
            if match:
                file_path = match.group(1)
                line_num = int(match.group(2))
                errors.append((file_path, line_num))
                print(f"Parsed: {file_path}:{line_num}")
            else:
                print(f"Could not parse: {line}")
        
        return errors
    except Exception as e:
        print(f"Error getting build output: {e}")
        return []

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
            
            # Skip if line doesn't contain ref/out with property access
            if not re.search(r'\b(ref|out)\s+[A-Za-z_]\w*\.[A-Za-z_]\w*', line):
                continue
            
            # Common patterns for serialization calls with properties
            patterns = [
                # Simple: Serializer.Serialize(ref PropertyName);
                (r'(\s*)([^.]*Serializer)\.Serialize\s*\(\s*(ref|out)\s+([A-Za-z_]\w*)\s*\)\s*;', 
                 r'\1var temp_\4 = \4;\n\1\2.Serialize(\3 temp_\4);\n\1\4 = temp_\4;'),
                
                # With parameters: Serializer.Serialize(ref PropertyName, param1, param2, ...);
                (r'(\s*)([^.]*Serializer)\.Serialize\s*\(\s*(ref|out)\s+([A-Za-z_]\w*)\s*,([^)]*)\)\s*;', 
                 r'\1var temp_\4 = \4;\n\1\2.Serialize(\3 temp_\4,\5);\n\1\4 = temp_\4;'),
                
                # serializer.Serialize(ref this.Property)
                (r'(\s*)(.*?)\.Serialize\s*\(\s*(ref|out)\s+([^,)]+)\.([A-Za-z_]\w*)\s*\)', 
                 r'\1var temp_\5 = \4.\5;\n\1\2.Serialize(\3 temp_\5);\n\1\4.\5 = temp_\5;'),
                
                # serializer.SerializeMethod(ref this.Property, ...)
                (r'(\s*)(.*?)\.Serialize([A-Za-z]*)\s*\(\s*(ref|out)\s+([^,)]+)\.([A-Za-z_]\w*)\s*,([^)]*)\)', 
                 r'\1var temp_\6 = \5.\6;\n\1\2.Serialize\3(\4 temp_\6,\7);\n\1\5.\6 = temp_\6;'),
                
                # Other serializer methods with ref/out properties
                (r'(\s*)(.*?)\.([A-Za-z]+)\s*\(\s*(ref|out)\s+([^,)]+)\.([A-Za-z_]\w*)\s*\)', 
                 r'\1var temp_\6 = \5.\6;\n\1\2.\3(\4 temp_\6);\n\1\5.\6 = temp_\6;'),
            ]
            
            original_line = line
            for pattern, replacement in patterns:
                match = re.search(pattern, line)
                if match:
                    # The replacement patterns already have the correct references
                    new_line = re.sub(pattern, replacement, line)
                    lines[line_idx] = new_line
                    changes_made += 1
                    print(f"Fixed CS0206 at line {line_num}: {original_line.strip()}")
                    break
        
        if changes_made > 0:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.writelines(lines)
            print(f"Made {changes_made} changes to {file_path}")
        
        return changes_made
        
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return 0

def main():
    print("Finding CS0206 errors...")
    errors = get_cs0206_errors()
    
    print(f"Debug: Raw errors list: {errors[:5]}...")  # Show first 5 for debugging
    
    if not errors:
        print("No CS0206 errors found!")
        return
    
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
    
    # Check if errors were fixed
    print("\nChecking remaining CS0206 errors...")
    remaining_errors = get_cs0206_errors()
    print(f"Remaining CS0206 errors: {len(remaining_errors)}")

if __name__ == "__main__":
    main()
