namespace NP.ObjectComparison.Benchmarks;

public class BenchmarkObject :  IEquatable<BenchmarkObject>, ICloneable
{
	public string? Id { get; set; }

	public string? Name { get; set; }

	public int Order { get; set; }

	/// <inheritdoc />
	public bool Equals(BenchmarkObject? other)
	{
		if (ReferenceEquals(null, other))
		{
			return false;
		}

		if (ReferenceEquals(this, other))
		{
			return true;
		}

		return Id == other.Id && Name == other.Name && Order == other.Order;
	}

	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj))
		{
			return false;
		}

		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		if (obj.GetType() != this.GetType())
		{
			return false;
		}

		return Equals((BenchmarkObject)obj);
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return HashCode.Combine(Id, Name, Order);
	}

	/// <inheritdoc />
	public object Clone()
	{
		return new BenchmarkObject
		{
			Id = (string?)Id?.Clone(),
			Name = (string?)Name?.Clone(),
			Order = Order
		};
	}

	public static bool operator ==(BenchmarkObject? left, BenchmarkObject? right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(BenchmarkObject? left, BenchmarkObject? right)
	{
		return !Equals(left, right);
	}

}