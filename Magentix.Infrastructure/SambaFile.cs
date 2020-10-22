using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Magentix.Infrastructure
{
    public class SambaFile
    {
        public SambaFile()
        {
        }

        public static void AppendAllText(string path, string contents)
        {
            File.AppendAllText(path, contents);
            SambaFile.GrantAccess(path);
        }

        public static void Copy(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName);
            SambaFile.GrantAccess(destFileName);
        }

        public static void Copy(string sourceFile, string destFile, bool overwrite)
        {
            File.Copy(sourceFile, destFile, overwrite);
            SambaFile.GrantAccess(destFile);
        }

        public static FileStream Create(string path)
        {
            return File.Create(path);
        }

        public static void Delete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (ArgumentNullException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (UnauthorizedAccessException)
            {
                if (File.Exists(path))
                {
                    throw;
                }
            }
        }

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static DateTime GetLastWriteTime(string path)
        {
            return File.GetLastWriteTime(path);
        }

        public static void GrantAccess(string fullPath)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
                DirectorySecurity accessControl = directoryInfo.GetAccessControl();
                accessControl.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                directoryInfo.SetAccessControl(accessControl);
            }
            catch (Exception)
            {
            }
        }

        public static bool HasWritePermission(string path)
        {
            FileSystemSecurity accessControl;
            bool flag;
            try
            {
                if (!SambaFile.Exists(path))
                {
                    accessControl = Directory.GetAccessControl(Path.GetDirectoryName(path));
                }
                else
                {
                    accessControl = File.GetAccessControl(path);
                }
                AuthorizationRuleCollection accessRules = accessControl.GetAccessRules(true, true, typeof(NTAccount));
                WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                bool flag1 = false;
                foreach (FileSystemAccessRule accessRule in accessRules)
                {
                    if ((int)(accessRule.FileSystemRights & FileSystemRights.Write) == 0)
                    {
                        continue;
                    }
                    if (accessRule.IdentityReference.Value.StartsWith("S-1-"))
                    {
                        if (!windowsPrincipal.IsInRole(new SecurityIdentifier(accessRule.IdentityReference.Value)))
                        {
                            continue;
                        }
                    }
                    else if (!windowsPrincipal.IsInRole(accessRule.IdentityReference.Value))
                    {
                        continue;
                    }
                    if (accessRule.AccessControlType != AccessControlType.Deny)
                    {
                        if (accessRule.AccessControlType != AccessControlType.Allow)
                        {
                            continue;
                        }
                        flag1 = true;
                    }
                    else
                    {
                        flag = false;
                        return flag;
                    }
                }
                flag = flag1;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public static void Move(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
            SambaFile.GrantAccess(destFileName);
        }

        public static Stream Open(string path, FileMode mode)
        {
            return File.Open(path, mode);
        }

        public static Stream OpenWrite(string path)
        {
            return File.OpenWrite(path);
        }

        public static byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }

        public static string[] ReadAllLines(string path, Encoding encoding)
        {
            return File.ReadAllLines(path, encoding);
        }

        public static string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public static void Touch(string path)
        {
            SambaFile.GrantAccess(path);
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
            SambaFile.GrantAccess(path);
        }

        public static void WriteAllText(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents, encoding);
            SambaFile.GrantAccess(path);
        }

        public static void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
            SambaFile.GrantAccess(path);
        }
    }
}
