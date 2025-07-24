public class Interpreter
{
    private Dictionary<string, Queue<int>> queues = new()
    {
        ["q"] = new Queue<int>(),
    };

    private Dictionary<string, Queue<object>> objQueues = new()
    {
        ["q."] = new Queue<object>()
    };

    private List<object> dqTemp = new();

    public Queue<object> printQueue => objQueues["q."];

    public void Execute(List<Node> nodes)
    {
        foreach (var node in nodes)
        {
            switch (node)
            {
                case DeclareNode d:
                    Declare(d.Name);
                    break;
                case EnqueueNode e:
                    Enqueue(e.Values, e.Name);
                    break;
                case DequeueNode dq:
                    Dequeue(dq.Name);
                    break;
                case OperationNode op:
                    Operate(op.Operator);
                    break;
                case PushNode p:
                    Push(p.Name);
                    break;
            }
        }
    }

    private void Declare(string name)
    {
        if (queues.ContainsKey(name) || objQueues.ContainsKey(name))
            throw new Exception($"queue {name} already exists");

        if (name == "q.")
        {
            objQueues[name] = new Queue<object>();
        }
        else
        {
            queues[name] = new Queue<int>();
        }
    }

    private void Enqueue(List<int> values, string name)
    {
        if (!queues.ContainsKey(name))
            throw new Exception($"cannot enqueue to undeclared queue: {name}");

        foreach (var val in values)
            queues[name].Enqueue(val);

        Console.WriteLine("enqueued: [" + string.Join(", ", queues[name]) + "] -> " + name + "\n");
    }

    private void Dequeue(string name)
    {
        dqTemp.Clear();

        if (queues.ContainsKey(name))
        {
            var q = queues[name];
            while (q.Count > 0)
            {
                dqTemp.Add(q.Dequeue());
            }

            Console.WriteLine("dequeued: " + name + " [" + string.Join(", ", queues[name]) + "] -> " + "[" + string.Join(", ", dqTemp) + "]\n");
        }
        else if (objQueues.ContainsKey(name))
        {
            var q = objQueues[name];
            while (q.Count > 0)
            {
                dqTemp.Add(q.Dequeue());
            }
        }
        else throw new Exception($"cannot dequeue from undeclared queue: {name}");
    }

    private void Operate(string op)
    {
        if (dqTemp.Count == 0)
            throw new Exception("no dequeued data found");

        var dqSnapshot = "[" + string.Join(", ", dqTemp) + "]";

        switch (op)
        {
            // Arithmetic //
            case "+":
            case "-":
            case "*":
            case "/":
                var intList = dqTemp.OfType<int>().ToList();

                if (intList.Count != dqTemp.Count)
                    throw new Exception("cannot operate on non-integer values");

                int result;

                switch (op)
                {
                    case "+":
                        dqTemp.Clear();
                        result = 0;
                        foreach (var val in intList)
                        {
                            result += val;
                        }
                        dqTemp.Add(result);
                        break;

                    case "-":
                        dqTemp.Clear();
                        if (intList.Count == 0)
                            throw new Exception("cannot subtract empty dequeued data");

                        result = intList[0];
                        for (int i = 1; i < intList.Count; i++)
                        {
                            result -= intList[i];
                        }
                        dqTemp.Add(result);
                        break;

                    case "*":
                        dqTemp.Clear();
                        result = 1;
                        foreach (var val in intList)
                        {
                            result *= val;
                        }
                        dqTemp.Add(result);
                        break;

                    case "/":
                        dqTemp.Clear();
                        if (intList.Count == 0)
                            throw new Exception("cannot subtract empty dequeued data");

                        result = intList[0];
                        for (int i = 1; i < intList.Count; i++)
                        {
                            if (intList[i] == 0)
                                throw new Exception("division by zero");

                            result /= intList[i];
                        }
                        dqTemp.Add(result);
                        break;
                }

                Console.WriteLine($"operation /{op}: {dqSnapshot} -> [{string.Join(", ", dqTemp)}]\n");
                break;
            // Arthmetic //

            // Operative //
            case "asc":
                var ascList = dqTemp.OfType<int>().ToList();

                if (ascList.Count != dqTemp.Count)
                    throw new Exception("cannot apply /asc on non-integer values");

                dqTemp.Clear();

                foreach (var i in ascList)
                {
                    if (i == 0)
                    {
                        dqTemp.Add(" ");
                    }
                    else if (i >= 1 && i <= 26)
                    {
                        char ch = (char)(i + 96);
                        dqTemp.Add(ch.ToString());
                    }
                    else
                    {
                        throw new Exception($"invalid alphabet index: {i}");
                    }
                }

                Console.WriteLine($"operation /{op}: {dqSnapshot} -> [{string.Join(", ", dqTemp)}]\n");

                break;

            case "c":
                var strList = dqTemp.OfType<string>().ToList();

                if (strList.Count != dqTemp.Count)
                    throw new Exception("cannot concatenate non-string values");

                dqTemp.Clear();

                var combined = string.Concat(strList);

                dqTemp.Add(combined);

                Console.WriteLine($"operation /{op}: {dqSnapshot} -> [{string.Join(", ", dqTemp)}]\n");

                break;

            case "b":
                var bList = dqTemp.OfType<int>().ToList();

                if (bList.Count != dqTemp.Count)
                    throw new Exception("cannot convert non-integer values to boolean");

                dqTemp.Clear();

                foreach (var i in bList)
                {
                    if (i == 0)
                    {
                        dqTemp.Add(false);
                    }
                    else if (i == 1)
                    {
                        dqTemp.Add(true);
                    }
                    else
                        throw new Exception($"cannot convert integer {i} to boolean; only 1 or 0 allowed");
                }

                Console.WriteLine($"operation /{op}: {dqSnapshot} -> [{string.Join(", ", dqTemp)}]\n");

                break;

            default:
                throw new Exception($"unknown operator: {op}");
        }
    }


    private void Push(string name)
    {
        if (dqTemp.Count == 0) return;

        if (objQueues.ContainsKey(name))
        {
            foreach (var val in dqTemp)
            {
                objQueues[name].Enqueue(val);
            }

            Console.WriteLine("pushed: [" + string.Join(", ", dqTemp) + "] -> " + name + "\n");
        }
        else if (queues.ContainsKey(name))
        {
            foreach (var val in dqTemp)
            {
                if (val is int i)
                {
                    queues[name].Enqueue(i);
                }
                else
                    throw new Exception($"cannot push non-integer values to integer queue: {name}");
            }

            Console.WriteLine("pushed: [" + string.Join(", ", dqTemp) + "] -> " + name + "\n");
        }
        else throw new Exception($"undeclared queue: {name}");

        dqTemp.Clear();
    }
}
