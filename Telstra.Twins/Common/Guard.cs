using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Telstra
{
    public static class Guard
    {
        /// <summary>
        /// Throws an <see cref="KeyNotFoundException"/> if the value for the given key is not found.
        /// </summary>
        public static void KeyNotFound(IDictionary dictionary, object key)
        {
            if (!dictionary.Contains(key))
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the given value is null.
        /// </summary>
        /// <param name="value">The value to check for nullity.</param>
        /// <param name="name">The name to use when throwing an exception, if necessary.</param>
        public static T NotNull<T>(T value, string name) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(name);
            return value;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the given value is negative.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="name">The name to use when throwing an exception, if necessary.</param>
        public static void NotNegative(int value, string name)
        {
            if (value < 0)
                throw new ArgumentException(name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the given value is null or
        /// <see cref="ArgumentException"/> if value is empty.
        /// </summary>
        /// <param name="value">The value to check for nullity or empty.</param>
        /// <param name="name">The name to use when throwing an exception, if necessary.</param>
        public static string NotNullOrEmpty(string value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
            if (value == string.Empty)
                throw new ArgumentException("Value is empty", name);
            return value;
        }

        public static string MaxMinLength(int smallestLength, int largestLength, string value, string name)
        {
            Guard.NotNullOrEmpty(value, name);
            var stringLength = value.Length;
            if (stringLength < smallestLength || stringLength > largestLength)
                throw new ArgumentException("Value out of range", name);
            return value;
        }

        public static string MaxLength(int largestLength, string value, string name)
        {
            Guard.NotNullOrEmpty(value, name);
            var stringLength = value.Length;
            if (stringLength > largestLength)
                throw new ArgumentException("Value out of range", name);
            return value;
        }

        public static string MinLength(int minLength, string value, string name)
        {
            Guard.NotNullOrEmpty(value, name);
            var stringLength = value.Length;
            if (stringLength < minLength)
                throw new ArgumentException("Value out of range", name);
            return value;
        }

        public static string NullOrMaxLength(int maxLength, string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value out of range", name);
            if (value.Length > maxLength)
                throw new ArgumentException("Value out of range", name);
            return value;
        }

        public static string NullOrMinLength(int minLength, string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value out of range", name);
            if (value.Length < minLength)
                throw new ArgumentException("Value out of range", name);
            return value;
        }

        public static decimal Decimal(string amount, string name)
        {
            Guard.NotNullOrEmpty(amount, name);
            if (!decimal.TryParse(amount, out decimal convertedAmount))
            {
                throw new ArgumentException("Value is not a decimal");
            }
            return convertedAmount;
        }

        public static int Int(string amount, string name)
        {
            Guard.NotNullOrEmpty(amount, name);
            if (!int.TryParse(amount, out var convertedAmount))
            {
                throw new ArgumentException("Value is not an int");
            }
            return convertedAmount;
        }

        public static void PositiveDecimalAmount(string amount, string name)
        {
            var convertedAmount = Guard.Decimal(amount, name);
            if (convertedAmount <= 0)
            {
                throw new ArgumentException("Value is not a positive decimal");
            }
        }

        public static void PositiveIntAmount(string amount, string name)
        {
            var convertedAmount = Guard.Int(amount, name);
            if (convertedAmount <= 0)
            {
                throw new ArgumentException("Value is not a positive int");
            }
        }

        public static T ValidReferenceValue<T>(T value, T expectedValue, string name) where T : IEquatable<T>
        {
            if (value == null)
                throw new ArgumentNullException(name);
            if (value.Equals(default(T)))
                throw new ArgumentException("Value is empty", name);
            if (!value.Equals(expectedValue))
                throw new ArgumentException("Value is Invalid", name);
            return value;
        }

        public static T ValidValue<T>(T value, T expectedValue, string name) where T : struct
        {
            if (value.Equals(default(T)))
                throw new ArgumentException("Value is not set", name);
            if (!value.Equals(expectedValue))
                throw new ArgumentException("Value is Invalid", name);
            return value;
        }

        public static void ValidEmail(string emailAddress, string emailAddressName)
        {
            var emailAddressAttribute = new EmailAddressAttribute();
            if (!emailAddressAttribute.IsValid(emailAddress))
            {
                throw new ArgumentException("Invalid email address", emailAddressName);
            }
        }

        public static T AllowedValue<T>(T value, T[] expectedValues, string name) where T : struct
        {
            if (value.Equals(default(T)))
                throw new ArgumentException("Value is not set", name);
            if (!expectedValues.Contains(value))
            {
                throw new ArgumentException("Value is not Invalid", name);
            }
            return value;
        }
    }
}