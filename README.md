# nQ
A queue-based esolang where all data flows through queues using simple commands. Currently, nQ supports integer queues, basic arithmetic, and string conversions.

## Table of Contents
- [Syntax](#syntax)
- [Examples](#examples)

## Syntax
### Basic Commands
- Declaring Variables:
  - All variables are default integer queues and are declared with `@varName;`
  - `q` (main integer queue) and `q.` (printing/output queue) are reserved queues.
    
- Enqueue: `[1, 2, 3]nq;` -- enqueue numbers 1, 2, & 3 into queue the `q` using the `n` enqueue command.
- Dequeue: `dq;` -- dequeue from queue `q` into a temporary list using the `d` dequeue command.
- Operations: `/+`, `/-`, `/asc`, etc. -- operators that only operate on dequeued data.
- Pushing: `>`-- pushing command enqueues dequeued data into a queue. `> q.;` pushes dequeued data into print queue for result.

## Examples
### Simple Arthmetic

`[1, 2, 3]nq; dq/+ > q.;` prints 6

### Simple String Output
`[8, 5, 12, 12, 15]nq; dq/asc/c > q.;` prints hello
- /asc converts numbers 1-26 into lower case ASCII characters.
- /c contatenates all characters into single string.
