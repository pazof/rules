/*----------------------------------------------------------------------
Compiler Generator Coco/R,
Copyright (c) 1990, 2004 Hanspeter Moessenboeck, University of Linz
extended by M. Loeberbauer & A. Woess, Univ. of Linz
with improvements by Pat Terry, Rhodes University

This program is free software; you can redistribute it and/or modify it 
under the terms of the GNU General Public License as published by the 
Free Software Foundation; either version 2, or (at your option) any 
later version.

This program is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License 
for more details.

You should have received a copy of the GNU General Public License along 
with this program; if not, write to the Free Software Foundation, Inc., 
59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.

As an exception, it is allowed to write an extension of Coco/R that is
used as a plugin in non-free software.

If not otherwise stated, any source code generated by Coco/R (other than 
Coco/R itself) does not fall under the GNU General Public License.
-----------------------------------------------------------------------*/
using System.Collections.Generic;
using System.IO;



using System;

namespace rules {



public class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _groupMark = 2;
	public const int _userMark = 3;
	public const int _and = 4;
	public const int _or = 5;
	public const int _not = 6;
	public const int _allow = 7;
	public const int _deny = 8;
	public const int _all = 9;
	public const int _semicolon = 10;
	public const int _from = 11;
	public const int maxT = 16;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public DefinitionSet Definitions;
public RuleSet Rules;


public UserMatch FindGroup(string gid)
{
    if (!Definitions.ContainsKey(gid))
        throw new InvalidRuleException("this rule id doesn't exist'");
    return Definitions[gid];
}

public void CreateGroup(string gid, UserMatch exp)
{
    if (Definitions.ContainsKey(gid))
        throw new InvalidRuleException("this rule id already exists'");
    Definitions.Add(gid, exp);
}




	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void userId(out string id) {
		Expect(3);
		Expect(1);
		id = t.val; 
	}

	void groupId(out string id) {
		Expect(2);
		Expect(1);
		id = t.val; 
	}

	void expression(out UserMatch exp) {
		exp = null; 
		if (la.kind == 3) {
			string uid; 
			userId(out uid);
			exp = new SingleUserMatch(uid); 
		} else if (la.kind == 2) {
			string gid; 
			groupId(out gid);
			exp = FindGroup(gid); 
		} else if (StartOf(1)) {
			UserMatch left, right; 
			expression(out left);
			Expect(4);
			expression(out right);
			exp = left.And(right); 
		} else if (StartOf(1)) {
			UserMatch left, right; 
			expression(out left);
			Expect(5);
			expression(out right);
			exp = left.And(right); 
		} else if (la.kind == 6) {
			UserMatch exp1; 
			Get();
			expression(out exp1);
			exp = exp1.Not(); 
		} else if (la.kind == 12) {
			UserMatch exp2; 
			Get();
			expression(out exp2);
			Expect(13);
			exp = exp2; 
		} else SynErr(17);
	}

	void definition() {
		UserMatch def = new UserMatchUnion(); UserMatch exp; 
		Expect(1);
		CreateGroup(t.val, def); 
		Expect(14);
		expression(out exp);
		def=def.Or(exp); 
		while (la.kind == 15) {
			Get();
			expression(out exp);
			def=def.Or(exp); 
		}
		Expect(10);
	}

	void rule(out Rule ruleItem) {
		UserMatch exp; bool allow=false; 
		if (la.kind == 7) {
			Get();
			allow = true; 
		} else if (la.kind == 8) {
			Get();
			allow = false; 
		} else SynErr(18);
		Expect(11);
		expression(out exp);
		ruleItem = allow ? (Rule) new AllowingRule(exp) : new DenyingRule(exp) ; 
		Expect(10);
	}

	void rules() {
		Rule ruleItem; 
		while (la.kind == 1) {
			definition();
		}
		while (la.kind == 7 || la.kind == 8) {
			rule(out ruleItem);
			Rules.Add(ruleItem); 
		}
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		rules();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x},
		{_x,_x,_T,_T, _x,_x,_T,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "groupMark expected"; break;
			case 3: s = "userMark expected"; break;
			case 4: s = "and expected"; break;
			case 5: s = "or expected"; break;
			case 6: s = "not expected"; break;
			case 7: s = "allow expected"; break;
			case 8: s = "deny expected"; break;
			case 9: s = "all expected"; break;
			case 10: s = "semicolon expected"; break;
			case 11: s = "from expected"; break;
			case 12: s = "\"(\" expected"; break;
			case 13: s = "\")\" expected"; break;
			case 14: s = "\":\" expected"; break;
			case 15: s = "\",\" expected"; break;
			case 16: s = "??? expected"; break;
			case 17: s = "invalid expression"; break;
			case 18: s = "invalid rule"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
}