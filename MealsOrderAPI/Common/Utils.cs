using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
//using AutoMapper;
using MessagePack;
using MealsOrderAPI.Models;

namespace MealsOrderAPI.Common {
    /// <summary>
    /// Simple utility functions
    /// </summary>
    public static class Utils {
        public static MessagePackSerializerOptions Lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

        /// <summary>
        /// LZ4 compression algorithm compress string to byte array.
        /// </summary>
        /// <param name="input">target string to compress</param>
        /// <returns>compressed byte array</returns>
        public static byte[] CompressStringToByteArray(string input){
            return MessagePackSerializer.Serialize(input, Lz4Options);
        }

        /// <summary>
        /// LZ4 compression algorithm decompress byte array to original string.
        /// </summary>
        /// <param name="input">compressed byte array</param>
        /// <returns>original string</returns>
        public static string DecompressByteArrayToString(byte[] input) {
            return MessagePackSerializer.Deserialize<string>(input, Lz4Options);
        }

        public static bool IsAny<T>(this IEnumerable<T> data) {
            return data != null && data.Any();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> data) {
            return data == null || !data.Any();
        }
}
}
