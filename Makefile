CONFIGURATION=Debug
COCO=Coco
MSBUILD=msbuild
NUNITCONSOLE=nunit-console
NUGET=nuget

all: rules/bin/$(CONFIGURATION)/rules.dll test/bin/$(CONFIGURATION)/test.dll

rules/Parser.cs: rules/Rules.atg
	$(COCO) rules/Rules.atg

rules/bin/$(CONFIGURATION)/rules.dll: rules/Parser.cs rules/InvalidRuleException.cs rules/RuleSetParser.cs rules/SingleUserMatch.cs rules/UserMatch.cs rules/UserMatchNot.cs rules/UserMatchUnion.cs rules/UserMatchIntersection.cs rules/Properties/AssemblyInfo.cs
	$(MSBUILD) /p:Configuration=$(CONFIGURATION)

test/bin/$(CONFIGURATION)/test.dll: test/Test.cs rules/bin/$(CONFIGURATION)/rules.dll
	$(MSBUILD) /p:Configuration=$(CONFIGURATION)

test: rules/bin/$(CONFIGURATION)/rules.dll test/bin/$(CONFIGURATION)/test.dll
	$(NUNITCONSOLE) test/bin/$(CONFIGURATION)/test.dll

pack:
	$(NUGET) pack rules/rules.csproj

.PHONY: test
