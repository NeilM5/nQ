# nQ
A queue-based esolang where all data flows through queues using simple commands. Currently, nQ supports integer queues, basic arithmetic, and string conversions.

---

## Features
- Enqueue and Dequeue data from one queue to another
- Store data in built-in main integer queue: `q`
- Apply operations (+, -, etc.) to dequeued data
- Apply type conversions (int to string, char to string, etc.)
- Output data using built-in print queue: `q.`

---

## How to Run
### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (version 6 or later)
### Build and Run
Clone the repository
```
git clone https://github.com/NeilM5/nQ.git
```
```
cd nQ
```
Then run it using:
```
dotnet run
```
This will run the Main method, launching the nQ REPL which looks like this:
```
>
```
You can now type nQ commands in the prompt!

---

## Syntax Overview
- `@queue_name;` - Declare a queue variable (default integer)
- `[1, 2, 3, ...]nq;` - Enqueues values to built-in main queue 'q'
- `dq;` - Dequeues values from 'q' into a temp list
- `/+`, `/-`, `/*`, `//`, etc. - Apply operations to deuqueued data
- `> q.;` - Push dequeued data into a queue (in this case, data is pushed to `q.` which is a built-in printing queue for output)
- `/asc`, `/c`, etc. - Type conversion operators

---

## Examples
### Hello World
```
[8, 5, 12, 12, 15, 0, 23, 15, 18, 12, 4]nq;
dq/asc/c > q.;
```
Prints: hello world
- Integer data 1-26 is converted into lower-case alphabet ASCII code and converted to chars with `/asc`
- `/c` concatenates chars into one string
- 0 represents space
