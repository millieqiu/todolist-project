using System.ComponentModel.DataAnnotations;

namespace todoAPP.ValidationAttributes;

public class GuidRequiredAttribute : ValidationAttribute
{
	public override bool IsValid(object? value)
	{
		var guidValue = value as Guid?;
		if (guidValue == null || guidValue == Guid.Empty)
		{
			return false;
		}
		return true;
	}
}