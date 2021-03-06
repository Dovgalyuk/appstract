﻿#region Copyright (C) 2009-2010 Simon Allaeys

/*
    Copyright (C) 2009-2010 Simon Allaeys
 
    This file is part of AppStract

    AppStract is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    AppStract is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with AppStract.  If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using HostRegistry = Microsoft.Win32.Registry;

namespace AppStract.Engine.Virtualization.FileSystem
{
  /// <summary>
  /// Provides helper methods for querying and manipulating the host's file system.
  /// </summary>
  public static class HostFileSystem
  {

    #region Public Types

    /// <summary>
    /// Defines native file management functions.
    /// </summary>
    public static class NativeMethods
    {

      /// <summary>
      /// Creates or opens a file or I/O device.
      /// </summary>
      /// <param name="fileName">The name of the file or device to be created or opened.</param>
      /// <param name="desiredAccess">The requested access to the file or device.</param>
      /// <param name="shareMode">The requested sharing mode of the file or device.</param>
      /// <param name="securityAttributes">
      /// A pointer to a SECURITY_ATTRIBUTES structure that contains two separate but related data members:
      /// an optional security descriptor, and a Boolean value that determines whether the returned handle can be inherited by child processes.
      /// </param>
      /// <param name="creationDisposition">An action to take on a file or device that exists or does not exist.</param>
      /// <param name="flagsAndAttributes">The file or device attributes and flags.</param>
      /// <param name="templateFile">An optional valid handle to a template file with the GENERIC_READ access right. The template file supplies file attributes and extended attributes for the file that is being created.</param>
      /// <returns></returns>
      [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
      public static extern IntPtr CreateFile(
          string fileName,
          FileAccessRightFlags desiredAccess,
          FileShareModeFlags shareMode,
          IntPtr securityAttributes,
          FileCreationDisposition creationDisposition,
          FileFlagsAndAttributes flagsAndAttributes,
          IntPtr templateFile);

      /// <summary>
      /// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
      /// </summary>
      /// <param name="fileName">The name of the module. This can be a library module (a .dll file) or an executable module (an .exe file).</param>
      /// <param name="file">This parameter is reserved for future use. It must be NULL.</param>
      /// <param name="flags">The action to be taken when loading the module.</param>
      /// <returns>If the function succeeds, the return value is a handle to the loaded module. Otherwise, NULL.</returns>
      [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
      public static extern IntPtr LoadLibraryEx(string fileName, IntPtr file, ModuleLoadFlags flags);

      /// <summary>
      /// Creates a new directory.
      /// If the underlying file system supports security on files and directories,
      /// the function applies a specified security descriptor to the new directory.
      /// </summary>
      /// <param name="pathName">The path of the directory to be created.</param>
      /// <param name="securityAttributes">
      /// A pointer to a SECURITY_ATTRIBUTES structure.
      /// The lpSecurityDescriptor member of the structure specifies a security descriptor for the new directory.
      /// If securityAttributes is NULL, the directory gets a default security descriptor.
      /// The ACLs in the default security descriptor for a directory are inherited from its parent directory.
      /// </param>
      /// <returns></returns>
      [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
      public static extern bool CreateDirectory(
          string pathName,
          NativeSecurityAttributes securityAttributes);

      /// <summary>
      /// Deletes an existing file.
      /// </summary>
      /// <param name="fileName">The name of the file to be deleted.</param>
      /// <returns>If the function succeeds, the return value is nonzero.</returns>
      [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
      public static extern bool DeleteFile(
          string fileName);

      /// <summary>
      /// Deletes an existing empty directory.
      /// </summary>
      /// <param name="pathName">The path of the directory to be removed.</param>
      /// <returns>If the function succeeds, the return value is nonzero.</returns>
      [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
      public static extern bool RemoveDirectory(
          string pathName);

    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Returns all folders that are used as temporary folders in the current system.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<string> GetTemporaryFolders()
    {
      var folders = new List<string>(3);
      // Get user specific temporary folders.
      folders.Add(Path.GetTempPath());
      // Get system temporary folders.
      const string regSystemEnvironmentPath =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment\";
      var value = HostRegistry.GetValue(regSystemEnvironmentPath, "TEMP", null);
      if (value != null)
        folders.Add(Path.GetFullPath(value.ToString()));
      value = HostRegistry.GetValue(regSystemEnvironmentPath, "TMP", null);
      if (value != null)
        folders.Add(Path.GetFullPath(value.ToString()));
      return folders;
    }

    /// <summary>
    /// Returns the location of the common menu folder.
    /// </summary>
    /// <remarks>
    /// For retrieving the location of any other system special folders, <see cref="Environment.GetFolderPath"/> should be used.
    /// </remarks>
    /// <returns>The full path to the common menu folder, or null in case the path can't be retrieved.</returns>
    public static string GetCommonMenuFolder()
    {
      var value = HostRegistry.GetValue(
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\explorer\Shell Folders",
        "Common Start Menu", null);
      return value as string;
    }

    /// <summary>
    /// Tries to create the directory, specified by <paramref name="path"/>.
    /// </summary>
    /// <param name="path">Directory to create.</param>
    /// <returns>True if the directory is created; False, otherwise.</returns>
    public static bool TryCreateDirectory(string path)
    {
      try
      {
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        return true;
      }
      catch (IOException)
      {
        return false;
      }
    }

    #endregion

  }
}
