﻿//
//  Bool.cs
//  Sui-Unity-SDK
//
//  Copyright (c) 2024 OpenDive
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//

using Sui.Utilities;

namespace OpenDive.BCS
{
    /// <summary>
    /// Representation of `bool` in BCS.
    /// </summary>
    public class Bool : ReturnBase, ISerializable
    {
        /// <summary>
        /// The `bool` value.
        /// </summary>
        public bool Value { get; set; }

        public Bool(bool value) => this.Value = value;

        public void Serialize(Serialization serializer) => serializer.SerializeBool(this.Value);

        public static Bool Deserialize(Deserialization deserializer) => new Bool(deserializer.DeserializeBool());

        public override string ToString() => this.Value.ToString();

        public override bool Equals(object other)
        {
            if (other is not Bool && other is not bool)
                return this.SetError<bool, BcsError>(false, "Compared object is not a BCS Bool nor a C# bool.", other);

            bool other_bool;

            if (other is Bool value)
                other_bool = value.Value;
            else
                other_bool = (bool)other;

            return this.Value == other_bool;
        }


        public override int GetHashCode() => base.GetHashCode();
    }
}