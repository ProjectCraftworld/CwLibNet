#!/usr/bin/env python3
"""
CwLibNet Migration Script
=========================

This script implements the migration strategy outlined in CwLibNet_Migration_Plan.md
to convert CwLibNet_4_HUB into a compatible version while preserving the authentic
LittleBigPlanet serialization method.

Key Goals:
1. Preserve the authentic LBP Method architecture
2. Add Unity compatibility layers where needed
3. Ensure .NET Standard 2.1 compatibility
4. Maintain all current functionality

Usage:
    python migrate_to_hub.py [--dry-run] [--backup] [--verbose]
"""

import os
import re
import shutil
import argparse
from pathlib import Path
from typing import List, Dict, Tuple, Optional
import logging

class MigrationConfig:
    """Configuration for the migration process."""
    
    def __init__(self, base_path: str):
        self.base_path = Path(base_path)
        self.cwlibnet_path = self.base_path / "CwLibNet"
        self.hub_path = self.base_path / "CwLibNet_4_HUB"
        self.backup_path = self.base_path / "backup"
        
        # File patterns to process
        self.cs_files = "**/*.cs"
        self.csproj_files = "**/*.csproj"
        
        # Exclude patterns
        self.exclude_dirs = {"bin", "obj", ".git", ".vs"}
        
    def get_source_files(self) -> List[Path]:
        """Get all C# source files to process."""
        files = []
        for pattern in [self.cs_files]:
            files.extend(self.hub_path.glob(pattern))
        
        # Filter out excluded directories
        return [f for f in files if not any(exclude in f.parts for exclude in self.exclude_dirs)]

class UnityCompatibilityLayer:
    """Handles Unity compatibility transformations."""
    
    # Unity type mappings that preserve authenticity
    UNITY_TYPE_MAPPINGS = {
        # Keep System.Numerics for core data, add Unity bridges
        "Vector3": "Vector4",  # Unity Vector3 -> System.Numerics Vector4 (with w=1)
        "Vector2": "Vector2",  # Compatible
        "Quaternion": "Quaternion",  # Compatible  
        "Matrix4x4": "Matrix4x4",  # Compatible
        # Add Unity bridge classes for complex types
        "Mesh": "UnityMeshBridge",
        "Material": "UnityMaterialBridge",
        "Texture2D": "UnityTextureBridge"
    }
    
    UNITY_COMPATIBILITY_IMPORTS = [
        "using UnityEngine;",
        "using System.Numerics;",
        "using CwLibNet.Unity.Bridges;"
    ]

class LegacySerializationMigrator:
    """Handles migration of legacy serialization patterns to authentic LBP Method."""
    
    def __init__(self, config: MigrationConfig):
        self.config = config
        self.unity_layer = UnityCompatibilityLayer()
        self.logger = logging.getLogger(__name__)
        
    def create_backup(self) -> bool:
        """Create backup of CwLibNet_4_HUB before migration."""
        try:
            if self.config.backup_path.exists():
                shutil.rmtree(self.config.backup_path)
            
            shutil.copytree(self.config.hub_path, self.config.backup_path)
            self.logger.info(f"Backup created at {self.config.backup_path}")
            return True
        except Exception as e:
            self.logger.error(f"Failed to create backup: {e}")
            return False
    
    def add_unity_bridge_classes(self) -> bool:
        """Add Unity compatibility bridge classes."""
        bridge_dir = self.config.hub_path / "Unity" / "Bridges"
        bridge_dir.mkdir(parents=True, exist_ok=True)
        
        # Unity Mesh Bridge
        mesh_bridge = '''using System;
using System.Numerics;
using UnityEngine;
using CwLibNet.Types.Meshes;
using Vector3 = UnityEngine.Vector3;
using Vector4 = System.Numerics.Vector4;

namespace CwLibNet.Unity.Bridges
{
    /// <summary>
    /// Unity compatibility bridge for RMesh.
    /// Preserves authentic LBP Method while providing Unity integration.
    /// </summary>
    [Serializable]
    public class UnityMeshBridge
    {
        [SerializeField] private RMesh _rmesh;
        
        public RMesh RMesh => _rmesh ??= new RMesh();
        
        public Mesh ToUnityMesh()
        {
            var mesh = new Mesh();
            
            if (RMesh.Vertices?.Length > 0)
            {
                // Convert System.Numerics.Vector4 to UnityEngine.Vector3
                var vertices = new Vector3[RMesh.Vertices.Length];
                for (int i = 0; i < RMesh.Vertices.Length; i++)
                {
                    var v = RMesh.Vertices[i];
                    vertices[i] = new Vector3(v.X, v.Y, v.Z);
                }
                mesh.vertices = vertices;
            }
            
            if (RMesh.TriangleData?.Length > 0)
            {
                mesh.triangles = RMesh.TriangleData;
            }
            
            return mesh;
        }
        
        public void FromUnityMesh(Mesh unityMesh)
        {
            if (unityMesh.vertices?.Length > 0)
            {
                RMesh.Vertices = new Vector4[unityMesh.vertices.Length];
                for (int i = 0; i < unityMesh.vertices.Length; i++)
                {
                    var v = unityMesh.vertices[i];
                    RMesh.Vertices[i] = new Vector4(v.x, v.y, v.z, 1.0f);
                }
            }
            
            if (unityMesh.triangles?.Length > 0)
            {
                RMesh.TriangleData = unityMesh.triangles;
            }
        }
    }
}'''
        
        bridge_file = bridge_dir / "UnityMeshBridge.cs"
        with open(bridge_file, 'w', encoding='utf-8') as f:
            f.write(mesh_bridge)
        
        self.logger.info("Unity bridge classes created")
        return True
    
    def preserve_authentic_serialization(self, file_path: Path) -> str:
        """Preserve authentic LBP Method serialization patterns."""
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Preserve the unified Serialize method pattern
        # Don't split into readBinary/writeBinary - this is the authentic LBP way
        
        # Add documentation about authenticity
        authenticity_comment = '''
    /// <summary>
    /// Uses the authentic LittleBigPlanet serialization method.
    /// This unified approach ensures read/write operations cannot go out of sync,
    /// as used by Media Molecule throughout the LBP series.
    /// </summary>'''
        
        # Add to Serialize methods
        content = re.sub(
            r'(\s+)(public\s+void\s+Serialize\s*\([^)]+\))',
            rf'\1{authenticity_comment}\1\2',
            content
        )
        
        return content
    
    def add_unity_compatibility_attributes(self, content: str) -> str:
        """Add Unity compatibility attributes while preserving core architecture."""
        
        # Add Unity serialization attributes to public fields
        content = re.sub(
            r'(\s+)(public\s+(?:(?:Vector4|Matrix4x4|int|float|bool|string)\??(?:\[\])?\s+\w+(?:\s*=\s*[^;]+)?);)',
            r'\1[SerializeField]\1\2',
            content
        )
        
        # Add System.Serializable to classes with Unity bridges
        content = re.sub(
            r'(public\s+class\s+\w+)',
            r'[System.Serializable]\n\1',
            content
        )
        
        return content
    
    def update_project_file(self) -> bool:
        """Update the .csproj file with Unity compatibility."""
        csproj_path = self.config.hub_path / "CwLibNet.csproj"
        
        with open(csproj_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Add Unity references conditionally
        unity_refs = '''
  <ItemGroup Condition="'$(DefineConstants)' == 'UNITY_2021_3_OR_NEWER'">
    <Reference Include="UnityEngine">
      <HintPath>$(UnityInstallPath)/Editor/Data/Managed/UnityEngine.dll</HintPath>
    </Reference>
  </ItemGroup>'''
        
        # Insert before closing Project tag
        content = content.replace('</Project>', f'{unity_refs}\n</Project>')
        
        with open(csproj_path, 'w', encoding='utf-8') as f:
            f.write(content)
        
        self.logger.info("Project file updated with Unity compatibility")
        return True
    
    def process_file(self, file_path: Path, dry_run: bool = False) -> bool:
        """Process a single C# file."""
        try:
            self.logger.debug(f"Processing {file_path}")
            
            # Preserve authentic serialization
            content = self.preserve_authentic_serialization(file_path)
            
            # Add Unity compatibility layers
            content = self.add_unity_compatibility_attributes(content)
            
            # Add proper imports if Unity bridges are used
            if "UnityMeshBridge" in content or "UnityMaterialBridge" in content:
                imports = "\n".join(self.unity_layer.UNITY_COMPATIBILITY_IMPORTS)
                content = f"{imports}\n{content}"
            
            if not dry_run:
                with open(file_path, 'w', encoding='utf-8') as f:
                    f.write(content)
            
            return True
            
        except Exception as e:
            self.logger.error(f"Failed to process {file_path}: {e}")
            return False
    
    def migrate(self, dry_run: bool = False, backup: bool = True) -> bool:
        """Execute the complete migration."""
        self.logger.info("Starting CwLibNet_4_HUB migration...")
        
        if backup and not dry_run:
            if not self.create_backup():
                return False
        
        # Get all source files
        source_files = self.config.get_source_files()
        self.logger.info(f"Found {len(source_files)} files to process")
        
        # Add Unity bridge classes
        if not dry_run:
            if not self.add_unity_bridge_classes():
                return False
        
        # Process each file
        successful = 0
        for file_path in source_files:
            if self.process_file(file_path, dry_run):
                successful += 1
        
        # Update project file
        if not dry_run:
            if not self.update_project_file():
                return False
        
        self.logger.info(f"Migration completed: {successful}/{len(source_files)} files processed")
        
        if dry_run:
            self.logger.info("DRY RUN - No files were actually modified")
        
        return successful == len(source_files)

def setup_logging(verbose: bool = False):
    """Setup logging configuration."""
    level = logging.DEBUG if verbose else logging.INFO
    logging.basicConfig(
        level=level,
        format='%(asctime)s - %(levelname)s - %(message)s',
        handlers=[
            logging.StreamHandler(),
            logging.FileHandler('migration.log')
        ]
    )

def main():
    """Main entry point."""
    parser = argparse.ArgumentParser(description='Migrate CwLibNet_4_HUB to Unity compatibility')
    parser.add_argument('--dry-run', action='store_true', help='Show what would be changed without modifying files')
    parser.add_argument('--no-backup', action='store_true', help='Skip creating backup')
    parser.add_argument('--verbose', action='store_true', help='Enable verbose logging')
    parser.add_argument('--base-path', default='.', help='Base path to CwLibNet project')
    
    args = parser.parse_args()
    
    setup_logging(args.verbose)
    logger = logging.getLogger(__name__)
    
    # Initialize configuration
    config = MigrationConfig(args.base_path)
    
    # Validate paths
    if not config.hub_path.exists():
        logger.error(f"CwLibNet_4_HUB path not found: {config.hub_path}")
        return 1
    
    # Create migrator and execute
    migrator = LegacySerializationMigrator(config)
    
    try:
        success = migrator.migrate(
            dry_run=args.dry_run, 
            backup=not args.no_backup
        )
        return 0 if success else 1
    except Exception as e:
        logger.error(f"Migration failed: {e}")
        return 1

if __name__ == "__main__":
    exit(main())
