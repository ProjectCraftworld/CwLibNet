#!/usr/bin/env python3

import os
import re
import sys

def convert_stream_calls(file_path):
    """Convert remaining stream instance method calls to static calls"""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        changes_made = []
        
        # Skip certain files that might have legitimate stream usage
        skip_patterns = [
            'FileArchive.cs',
            'FileDB.cs', 
            'SerializedResource.cs',
            'Compressor.cs'
        ]
        
        if any(pattern in file_path for pattern in skip_patterns):
            print(f"Skipping {file_path} - likely legitimate stream usage")
            return False, []
        
        # Pattern 1: stream.I32(value) -> Serializer.GetOutput().I32(value) 
        # But we need to be careful about context
        
        # Look for the pattern where stream is obtained from Serializer.GetOutput/GetInput
        # and then used for method calls
        
        lines = content.splitlines()
        modified_lines = []
        i = 0
        
        while i < len(lines):
            line = lines[i]
            
            # Look for stream assignment from Serializer
            stream_assignment_match = re.search(r'var\s+(\w+)\s*=\s*Serializer\.Get(Output|Input)\(\);', line)
            
            if stream_assignment_match:
                stream_var = stream_assignment_match.group(1)
                stream_type = stream_assignment_match.group(2)  # "Output" or "Input"
                
                modified_lines.append(line)
                i += 1
                
                # Now look ahead for uses of this stream variable
                while i < len(lines):
                    current_line = lines[i]
                    
                    # Check if this line uses the stream variable for method calls
                    method_pattern = rf'\b{re.escape(stream_var)}\.([A-Za-z][A-Za-z0-9]*)\('
                    method_match = re.search(method_pattern, current_line)
                    
                    if method_match:
                        method_name = method_match.group(1)
                        
                        # Skip certain methods that should stay as instance calls
                        skip_methods = ['Length', 'Seek', 'GetOffset', 'ToArray', 'GetCompressionFlags']
                        
                        if method_name not in skip_methods:
                            # Replace stream.Method( with Serializer.GetOutput().Method( or Serializer.GetInput().Method(
                            new_line = re.sub(
                                rf'\b{re.escape(stream_var)}\.{re.escape(method_name)}\(',
                                f'Serializer.Get{stream_type}().{method_name}(',
                                current_line
                            )
                            if new_line != current_line:
                                changes_made.append(f"Converted {stream_var}.{method_name}() to static Serializer.Get{stream_type}().{method_name}()")
                            modified_lines.append(new_line)
                        else:
                            modified_lines.append(current_line)
                    else:
                        # Check if we've left the scope where this stream is used
                        if (current_line.strip().startswith('}') or 
                            current_line.strip().startswith('else') or
                            re.search(r'var\s+\w+\s*=\s*Serializer\.Get(Output|Input)\(\);', current_line)):
                            modified_lines.append(current_line)
                            break
                        else:
                            modified_lines.append(current_line)
                    
                    i += 1
            else:
                modified_lines.append(line)
                i += 1
        
        new_content = '\n'.join(modified_lines)
        
        # Additional cleanup: convert remaining direct stream calls that follow the pattern
        # stream.Method() where stream is clearly from Serializer
        
        # Pattern: Look for remaining stream.Method calls and replace with Serializer calls
        remaining_patterns = [
            (r'stream\.I32\(', 'Serializer.GetOutput().I32('),
            (r'stream\.I16\(', 'Serializer.GetOutput().I16('),
            (r'stream\.U32\(', 'Serializer.GetOutput().U32('),
            (r'stream\.U16\(', 'Serializer.GetOutput().U16('),
            (r'stream\.U8\(', 'Serializer.GetOutput().U8('),
            (r'stream\.V3\(', 'Serializer.GetOutput().V3('),
            (r'stream\.V4\(', 'Serializer.GetOutput().V4('),
            (r'stream\.Bytes\(', 'Serializer.GetOutput().Bytes('),
            (r'stream\.Str\(', 'Serializer.GetOutput().Str('),
            (r'stream\.Boole\(', 'Serializer.GetOutput().Boole('),
        ]
        
        # Only apply these if the line contains evidence this is a Serializer context
        for pattern, replacement in remaining_patterns:
            if re.search(pattern, new_content):
                # Check if this appears in a Serializer context
                lines_to_check = new_content.splitlines()
                for line_idx, line in enumerate(lines_to_check):
                    if re.search(pattern, line):
                        # Look for context clues that this is in a Serializer method
                        context_start = max(0, line_idx - 10)
                        context_end = min(len(lines_to_check), line_idx + 5)
                        context = '\n'.join(lines_to_check[context_start:context_end])
                        
                        if ('Serializer.' in context and 
                            ('GetOutput()' in context or 'GetInput()' in context or 'IsWriting()' in context)):
                            new_content = re.sub(pattern, replacement, new_content)
                            changes_made.append(f"Converted {pattern} to static {replacement}")
                            break
        
        # Write the file if changes were made
        if new_content != original_content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(new_content)
            return True, changes_made
        else:
            return False, []
            
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return False, []

def find_files_with_stream_calls():
    """Find files that still have stream instance method calls"""
    files_with_streams = []
    
    cwlibnet_hub_dir = "/home/dylan/Documents/GitHub/CwLibNet/CwLibNet_4_HUB"
    
    for root, dirs, files in os.walk(cwlibnet_hub_dir):
        for file in files:
            if file.endswith('.cs'):
                file_path = os.path.join(root, file)
                try:
                    with open(file_path, 'r', encoding='utf-8') as f:
                        content = f.read()
                    
                    # Check for stream method calls
                    if any(pattern in content for pattern in [
                        'stream.I32(',
                        'stream.I16(',
                        'stream.U32(',
                        'stream.U16(',
                        'stream.U8(',
                        'stream.V3(',
                        'stream.V4(',
                        'stream.Bytes(',
                        'stream.Str(',
                        'stream.Boole('
                    ]):
                        files_with_streams.append(file_path)
                        
                except Exception as e:
                    print(f"Error reading {file_path}: {e}")
    
    return files_with_streams

def main():
    print("Converting remaining stream instance method calls to static calls...")
    
    files_to_convert = find_files_with_stream_calls()
    
    if not files_to_convert:
        print("No files found with stream instance calls!")
        return
    
    print(f"Found {len(files_to_convert)} files with stream instance calls:")
    for file_path in files_to_convert[:10]:  # Show first 10
        print(f"  {file_path}")
    if len(files_to_convert) > 10:
        print(f"  ... and {len(files_to_convert) - 10} more")
    
    total_files_changed = 0
    total_changes = 0
    
    for file_path in files_to_convert:
        changed, changes = convert_stream_calls(file_path)
        if changed:
            total_files_changed += 1
            total_changes += len(changes)
            print(f"\n✓ {file_path}")
            for change in changes:
                print(f"  - {change}")
    
    print(f"\n=== Stream Conversion Summary ===")
    print(f"Files changed: {total_files_changed}")
    print(f"Total changes: {total_changes}")
    
    if total_files_changed > 0:
        print("\n⚠️  Note: Stream operations may have complex context.")
        print("   Please review changes and test the build.")

if __name__ == "__main__":
    main()
