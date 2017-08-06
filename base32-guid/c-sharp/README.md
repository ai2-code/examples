# Encode a guid with base32 encoding in C# / .Net

## [C# Example 1](base32example1)

Voorbeeld om een guid met base32 encoding naar een string van 26 karakters om te zetten, zodat deze te gebruiken is als referentie.

	Guid guid = Guid.NewGuid();
	string encodedGuid = guid.ToBase32String();


