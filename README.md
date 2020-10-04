# rules

cette librairie permet de construire des règles à partir de textes du style :

````
FAMILY: @Paul, @Soraya, @Ninou,@Sherine;
allow from #FAMILY;
````

On évalue ces règles par la suite,
en les compilant, et en donnant un nom à tester en argument :

````
parser = new RuleSetParser(false); // par défaut, dénier
parser.Parse("FAMILY: @Paul, @Soraya, @Ninou, @Sherine;");
parser.Parse("allow from #FAMILY;");
````

À ce niveau, `parser.Rules` contient l'objet de type `RuleSet` utile.

````
bool sorayaDenied = parser.Rules.Deny("Soraya"); // sorayaDenied est faux
bool sorayaAllowed = parser.Rules.Allow("Soraya"); // sorayaAllowed est vrai
````

Plus d'exemple sont visible depuis les tests unitaires.
