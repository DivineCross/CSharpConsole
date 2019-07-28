# Merger Design and Applications
設計Merger的基本概念是優化物件間資料合併的操作，
減少assignment statement左右欄位的重複出現以降低撰寫成本，
並將欄位指定的設定抽取出來得到與其他操作邏輯的程式碼分離的效果

## Merging Simple Object
基本型是物件中僅包含value type的property，處理上等同於一般的assignment，如`l.X = r.X`

```cs
private class Model
{
    public int? Int1 { get; set; }

    public string Str1 { get; set; }

    public decimal? Dec1 { get; set; }
}

[Fact]
private void MergeSimpleObject()
{
    var l = new Model {
        Int1 = null,
        Str1 = "Hello",
        Dec1 = 888m,
    };
    var r = new Model {
        Int1 = 87,
        Str1 = "World",
        Dec1 = 777m,
    };

    var merger = Merger<Model>.Create()
        .Prop(_ => _.Int1)
        .Prop(_ => _.Str1);
    merger.Merge(l, r);

    Assert.Equal(l.Int1, r.Int1);
    Assert.Equal(l.Str1, r.Str1);
    Assert.NotEqual(l.Dec1, r.Dec1);
}
```

## Strongly Typed and Method Chaining
Merger設定方式，利用genric method支援strongly typed的寫法，
以利[intelligent code completion](https://en.wikipedia.org/wiki/Intelligent_code_completion)；
並加入[method chaining](https://en.wikipedia.org/wiki/Method_chaining)的設計提高撰寫的流暢度

```cs
public class Merger<T>
    where T : new()
{
    // generic 'TProp' for the property type in 'expr'
    public Merger<T> Prop<TProp>(Expression<Func<T, TProp>> expr)
    {
        ...
        // return 'this' for method chaining
        return this;
    }
}
```

## Merging Nested Object
僅支援同義於assignment的property合併不能滿足需要，
實際上property很容易是reference type，
因此需要支援nested object的功能，
設計透過指定property合併需要使用另一個merger的方式來實現

```cs
private class Person
{
    public int Age { get; set; }

    public Lion Lion { get; set; }
}

private class Lion
{
    public string Name { get; set; }

    public int Age { get; set; }
}

[Fact]
private void MergeNestedObject()
{
    var l = new Person {
        Age = 94,
        Lion = new Lion {
            Name = "Happy",
            Age = 8,
        },
    };
    var r = new Person {
        Age = 87,
        Lion = new Lion {
            Name = "Charlie",
            Age = 7,
        },
    };

    // outer type 'Person'
    var merger = Merger<Person>.Create()
        .Prop(_ => _.Age)
        // inner type 'Lion'
        .Prop(_ => _.Lion, Merger<Lion>.Create()
            .Prop(_ => _.Name));

    merger.Merge(l, r);

    Assert.Equal(l.Age, r.Age);
    Assert.Equal(l.Lion.Name, r.Lion.Name);
    Assert.NotEqual(l.Lion.Age, r.Lion.Age);
}
```

## Creating Setter by Expression
merge的行為是將property的值取出後再放入另一個property，因此需要以程式建立getter以及setter，
getter的做法較為簡單，但setter則需要利用expression進行重組

```cs
public static Action<TModel, TProp> CreateSetter<TModel, TProp>(Expression<Func<TModel, TProp>> expr)
{
    // Support only the lambda expression of the form 'x => x.Member'.

    // x => 'x.Member'
    var memberExpr = expr.Body as MemberExpression;
    // 'x' => x.Member
    var paramExpr = expr.Parameters[0];

    if (memberExpr == null) return null;

    // 'x'.Member
    var memberExprExpr = memberExpr.Expression;
    // x.'Member'
    var member = memberExpr.Member;

    if (paramExpr != memberExprExpr) return null;

    // Make '(x, v) => x.Member = v'.

    // ('x', v) => x.Member = v
    var paramX = paramExpr;
    // (x, 'v') => x.Member = v
    var paramV = Expression.Parameter(typeof(TProp));

    // (x, v) => 'x.Member' = v
    var left = memberExpr;
    // (x, v) => x.Member = 'v'
    var right = paramV;
    // (x, v) => 'x.Member = v'
    var assign = Expression.Assign(left, right);

    // '(x, v) => x.Member = v'
    var lambda = Expression.Lambda<Action<TModel, TProp>>(assign, paramX, paramV);
    var compiled = lambda.Compile();

    return compiled;
}
```

## Merging List
除了nested object，還可能會需要操作集合型的property，
集合型的type有許多種，而merge有異動集合項目的需要，
為了降低實作的複雜度，只設計支援IList介面的集合型property。
List合併使用key可自行指定，合併的類型則模擬SQL join的
[四種類型(inner, left, right, full)](https://stackoverflow.com/questions/5706437/whats-the-difference-between-inner-join-left-join-right-join-and-full-join)

```cs
// property without key selector, use index as the key for merging
public Merger<T> PropInner<TProp, TItem>(
    Expression<Func<T, TProp>> expr,
    Merger<TItem> merger)
    where TProp : IList<TItem>, new()
    where TItem : new() {}

// property with key selector, use the selected key as the key for merging
public Merger<T> PropInner<TProp, TItem, TKey>(
    Expression<Func<T, TProp>> expr,
    Func<TItem, TKey> keySelector,
    Merger<TItem> merger)
    where TProp : IList<TItem>, new()
    where TItem : new() {}
```

```cs
private class Person
{
    public List<Lion> Lions { get; set; }
}

private class Lion
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }
}

[Fact]
private void MergeList()
{
    var l = new Person {
        Lions = new List<Lion> {
            new Lion { Id = 3, Age = 6, Name = "Loki" },
            new Lion { Id = 4, Age = 6, Name = "Nova" },
            new Lion { Id = 5, Age = 6, Name = "Zeus" },
        },
    };
    var r = new Person {
        Lions = new List<Lion> {
            new Lion { Id = 4, Age = 8, Name = "Leo" },
            new Lion { Id = 5, Age = 8, Name = "King" },
            new Lion { Id = 6, Age = 8, Name = "Apollo" },
        },
    };

    var merger = Merger<Person>.Create()
        // Person.Lions
        .PropInner(_ => _.Lions, _ => _.Id, Merger<Lion>.Create()
            // Lion.Name
            .Prop(_ => _.Name));

    merger.Merge(l, r);
    var lion4 = l.Lions[0];
    var lion5 = l.Lions[1];

    Assert.Equal(2, l.Lions.Count);

    Assert.Equal(4, lion4.Id);
    Assert.Equal(6, lion4.Age);
    Assert.Equal("Leo", lion4.Name);

    Assert.Equal(5, lion5.Id);
    Assert.Equal(6, lion5.Age);
    Assert.Equal("King", lion5.Name);
}
```
