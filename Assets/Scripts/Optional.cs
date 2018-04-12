using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Optional<T>
{
    private T contents;
    private bool hasItem;
    private Optional(T obj)
    {
        if (obj == null)
        {
            this.hasItem = false;
        }
        else
        {
            this.contents = obj;
            this.hasItem = true;
        }
    }
    private Optional()
    {
        this.hasItem = false;
    }

    public T Get()
    {
        return this.contents;
    }

    public bool IsPresent()
    {
        return this.hasItem;
    }

    public static Optional<T> Of(T value)
    {
        return new Optional<T>(value);
    }
    public static Optional<T> Empty()
    {
        return new Optional<T>();
    }
}
