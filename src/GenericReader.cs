using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UsmapNET
{
	internal sealed class GenericStreamReader : GenericReader
	{
		private readonly Stream _stream;

		public override long Position
		{
			get => _stream.Position;
			set => _stream.Position = value;
		}

		public override long Size => _stream.Length;

		public GenericStreamReader(Stream stream)
		{
			_stream = stream;
		}

		public GenericStreamReader(string path)
		{
			_stream = File.OpenRead(path);
		}

		public override int ReadByte()
		{
			return _stream.ReadByte();
		}

		public override byte[] ReadBytes(int length)
		{
			var buffer = new byte[length];
			_stream.Read(buffer);
			return buffer;
		}

		public override T Read<T>()
		{
			var size = Unsafe.SizeOf<T>();
			var buffer = new byte[size];
			_stream.Read(buffer);
			return Unsafe.ReadUnaligned<T>(ref buffer[0]);
		}

		public override string ReadString(int length, Encoding encoding)
		{
			var buffer = new byte[length];
			_stream.Read(buffer);
			return encoding.GetString(buffer);
		}

		public override void Dispose()
		{
			_stream.Dispose();
		}

		public ValueTask DisposeAsync()
		{
			return _stream.DisposeAsync();
		}
	}

	internal sealed class GenericBufferReader : GenericReader
	{
		private readonly byte[] _buffer;
		private int _position;

		public override long Position
		{
			get => _position;
			set => _position = (int)value;
		}

		public override long Size => _buffer.Length;

		public GenericBufferReader(byte[] buffer)
		{
			_buffer = buffer;
		}

		public override int ReadByte()
		{
			return _buffer[_position++];
		}

		public override byte[] ReadBytes(int length)
		{
			var buffer = new byte[length];
			Unsafe.CopyBlockUnaligned(ref buffer[0], ref _buffer[_position], (uint)length);
			_position += length;
			return buffer;
		}

		public override T Read<T>()
		{
			var size = Unsafe.SizeOf<T>();
			var result = Unsafe.ReadUnaligned<T>(ref _buffer[_position]);
			_position += size;
			return result;
		}

		public override string ReadString(int length, Encoding encoding)
		{
			var result = encoding.GetString(_buffer, _position, length);
			_position += length;
			return result;
		}

		public override void Dispose() { }
	}

	internal abstract class GenericReader : IDisposable
	{
		public abstract long Position { get; set; }
		public abstract long Size { get; }
		public abstract int ReadByte();
		public abstract byte[] ReadBytes(int length);
		public abstract T Read<T>() where T : unmanaged;
		public abstract string ReadString(int length, Encoding encoding);
		public abstract void Dispose();
	}
}