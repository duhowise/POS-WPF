using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magentix.Infrastructure
{
    public struct ShortGuid
    {
        public readonly static ShortGuid Empty;

        private System.Guid _guid;

        private string _value;

        public System.Guid Guid
        {
            get
            {
                return this._guid;
            }
            set
            {
                if (value != this._guid)
                {
                    this._guid = value;
                    this._value = ShortGuid.Encode(value);
                }
            }
        }

        public string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if (value != this._value)
                {
                    this._value = value;
                    this._guid = ShortGuid.Decode(value);
                }
            }
        }

        static ShortGuid()
        {
            ShortGuid.Empty = new ShortGuid(System.Guid.Empty);
        }

        public ShortGuid(string value)
        {
            this._value = value;
            this._guid = ShortGuid.Decode(value);
        }

        public ShortGuid(System.Guid guid)
        {
            this._value = ShortGuid.Encode(guid);
            this._guid = guid;
        }

        public static System.Guid Decode(string value)
        {
            value = value.Replace("_", "/").Replace("-", "+");
            return new System.Guid(Convert.FromBase64String(string.Concat(value, "==")));
        }

        public static string Encode(string value)
        {
            return ShortGuid.Encode(new System.Guid(value));
        }

        public static string Encode(System.Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 22);
        }

        public override bool Equals(object obj)
        {
            if (obj is ShortGuid)
            {
                return this._guid.Equals(((ShortGuid)obj)._guid);
            }
            if (obj is System.Guid)
            {
                return this._guid.Equals((System.Guid)obj);
            }
            if (!(obj is string))
            {
                return false;
            }
            return this._guid.Equals(((ShortGuid)obj)._guid);
        }

        public override int GetHashCode()
        {
            return this._guid.GetHashCode();
        }

        public static ShortGuid NewGuid()
        {
            return new ShortGuid(System.Guid.NewGuid());
        }

        public static bool operator ==(ShortGuid x, ShortGuid y)
        {
            if ((object)x == null)
            {
                return (object)y == null;
            }
            return x._guid == y._guid;
        }

        public static implicit operator String(ShortGuid shortGuid)
        {
            return shortGuid._value;
        }

        public static implicit operator Guid(ShortGuid shortGuid)
        {
            return shortGuid._guid;
        }

        public static implicit operator ShortGuid(string shortGuid)
        {
            return new ShortGuid(shortGuid);
        }

        public static implicit operator ShortGuid(System.Guid guid)
        {
            return new ShortGuid(guid);
        }

        public static bool operator !=(ShortGuid x, ShortGuid y)
        {
            return !(x == y);
        }

        public override string ToString()
        {
            return this._value;
        }
    }
}
