using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NP.ObjectComparison.Tests.Mocks;

public class NotifyChangesTestObject : ICloneable, IEquatable<NotifyChangesTestObject>, INotifyPropertyChanged
{
	private string _firstName;
	private string _middleName;
	private string _lastName;
	private int _age;

	/// <inheritdoc />
	public event PropertyChangedEventHandler PropertyChanged;

	/// <summary>
	/// Used only for Tests.
	/// </summary>
	/// <returns></returns>
	public Delegate[] GetInvocationList() => PropertyChanged?.GetInvocationList();

	public string FirstName
	{
		get => _firstName;
		set
		{
			_firstName = value;
			OnPropertyChanged();
		}
	}

	public string MiddleName
	{
		get => _middleName;
		set
		{
			_middleName = value;
			OnPropertyChanged();
		}
	}

	public string LastName
	{
		get => _lastName;
		set
		{
			_lastName = value;
			OnPropertyChanged();
		}
	}

	public int Age
	{
		get => _age;
		set
		{
			_age = value;
			OnPropertyChanged();
		}
	}


	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	/// <inheritdoc />
	public object Clone()
	{
		return new NotifyChangesTestObject
		{
			FirstName = FirstName,
			MiddleName = MiddleName,
			LastName = LastName,
			Age = Age,
		};
	}


	/// <inheritdoc />
	public bool Equals(NotifyChangesTestObject other)
	{
		if (ReferenceEquals(null, other))
		{
			return false;
		}

		if (ReferenceEquals(this, other))
		{
			return true;
		}

		return FirstName == other.FirstName && MiddleName == other.MiddleName && LastName == other.LastName && Age == other.Age;
	}

	/// <inheritdoc />
	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj))
		{
			return false;
		}

		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		if (obj.GetType() != GetType())
		{
			return false;
		}

		return Equals((NotifyChangesTestObject)obj);
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return HashCode.Combine(FirstName, MiddleName, LastName, Age);
	}

	public static bool operator ==(NotifyChangesTestObject left, NotifyChangesTestObject right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(NotifyChangesTestObject left, NotifyChangesTestObject right)
	{
		return !Equals(left, right);
	}
}