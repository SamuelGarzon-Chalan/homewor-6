
public class Program
{
    public static void Main(string[] args)
    {
        // ======================= TESTS (do not change) =======================
        var empty = new BinarySearchTree();
        var one = new BinarySearchTree();
        one.Insert(7);
        var t1 = new BinarySearchTree();
        foreach (int v in new[] { 50, 30, 70, 20, 40, 80, 35, 45 }) t1.Insert(v);
        var line = new BinarySearchTree();
        foreach (int v in new[] { 10, 20, 30, 40, 50 }) line.Insert(v);
        // A tree built by hand that breaks the BST rule:
        // 50
        // / \
        // 30 70
        // \
        // 60 <- 60 is bigger than 50, but it is on 50's left side
        var badRoot = new Node(50);
        badRoot.Left = new Node(30);
        badRoot.Right = new Node(70);
        badRoot.Left.Right = new Node(60);
        var bad = new BinarySearchTree(badRoot);
        Console.WriteLine("== CountLeaves (BFS) ==");
        Console.WriteLine(t1.CountLeaves());
        Console.WriteLine(line.CountLeaves());
        Console.WriteLine(one.CountLeaves());
        Console.WriteLine(empty.CountLeaves());
        Console.WriteLine();
        Console.WriteLine("== Sum (DFS) ==");
        Console.WriteLine(t1.Sum());
        Console.WriteLine(line.Sum());
        Console.WriteLine(empty.Sum());
        Console.WriteLine();
        Console.WriteLine("== Height (DFS) ==");
        Console.WriteLine(t1.Height());
        Console.WriteLine(line.Height());
        Console.WriteLine(one.Height());
        Console.WriteLine(empty.Height());
        Console.WriteLine();
        Console.WriteLine("== IsValidBST (inorder) ==");
        Console.WriteLine(t1.IsValidBST());
        Console.WriteLine(line.IsValidBST());
        Console.WriteLine(bad.IsValidBST());
        Console.WriteLine(empty.IsValidBST());
        Console.WriteLine();
    }
}
// ======================= CLASSES =======================
public class Node
{
    public int Value;
    public Node? Left;
    public Node? Right;
    public Node(int value) { Value = value; }
}
public class BinarySearchTree
{
    private Node? root;

    public Node? Root => root;

    public BinarySearchTree(Node? startRoot = null)
    {
        root = startRoot;
    }

    public bool Insert(int value)
    {
        Node newNode = new Node(value);

        if (root == null)
        {
            root = newNode;
            return true;
        }

        Node temp = root;

        while (true)
        {
            if (newNode.Value == temp.Value)
                return false;

            if (newNode.Value < temp.Value)
            {
                if (temp.Left == null)
                {
                    temp.Left = newNode;
                    return true;
                }

                temp = temp.Left;
            }
            else
            {
                if (temp.Right == null)
                {
                    temp.Right = newNode;
                    return true;
                }

                temp = temp.Right;
            }
        }
    }

    // From class: BFS (level by level, uses a queue)
    public List<int> BFS()
    {
        var results = new List<int>();

        if (root == null)
            return results;

        var queue = new Queue<Node>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

            results.Add(currentNode.Value);

            if (currentNode.Left != null)
                queue.Enqueue(currentNode.Left);

            if (currentNode.Right != null)
                queue.Enqueue(currentNode.Right);
        }

        return results;
    }
    // TODO: return how many leaves the tree has.
    // Use a queue, the same way BFS does. An empty tree has 0 leaves.
    public int CountLeaves()
    {
        if (root == null)
            return 0;

        int leaves = 0;

        var queue = new Queue<Node>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

            // Una hoja es un nodo que no tiene hijos.
            if (currentNode.Left == null && currentNode.Right == null)
                leaves++;

            if (currentNode.Left != null)
                queue.Enqueue(currentNode.Left);

            if (currentNode.Right != null)
                queue.Enqueue(currentNode.Right);
        }

        return leaves;
    }
    // TODO: return the total of all values in the tree. An empty tree returns 0.
    // Use DFS: a Traverse local function, like DFSInOrder.
    public int Sum()
    {
        int total = 0;

        void Traverse(Node cn)
        {
            if (cn.Left != null)
                Traverse(cn.Left);

            total += cn.Value;

            if (cn.Right != null)
                Traverse(cn.Right);
        }

        if (root != null)
            Traverse(root);

        return total;
    }

    public List<int> DFSInOrder()
    {
        var results = new List<int>();

        void Traverse(Node cn)
        {
            if (cn.Left != null)
                Traverse(cn.Left);

            results.Add(cn.Value);

            if (cn.Right != null)
                Traverse(cn.Right);
        }

        if (root != null)
            Traverse(root);

        return results;
    }


    // TODO: return the height of the tree. Empty tree = -1. One node = 0.
    // Use recursion: the height of a node is 1 + the bigger height of its

    public int Height()
    
        {
            int H(Node? n)
            {
                if (n == null)
                    return -1;

                int leftHeight = H(n.Left);
                int rightHeight = H(n.Right);

                return 1 + Math.Max(leftHeight, rightHeight);
            }

            return H(root);
        }
    // TODO: return true if the tree follows the BST rule, false if not.
    // Hint: call DFSInOrder(). In a correct BST, every number in that list
    // is bigger than the number before it.
    public bool IsValidBST()
    {
        List<int> values = DFSInOrder();

        for (int i = 1; i < values.Count; i++)
        {
            if (values[i] <= values[i - 1])
                return false;
        }

        return true;
    }
}
/*  ======================= WRITTEN ANSWERS (Questions 1 to 3)
---- Question 1: BFS by hand ----

Step-up   Remove from the line   Join line after children have joined   Take off the line

Start             [52]                          []

1      52         [30, 75]                      [52]

2      30         [75, 18, 40]                  [52, 30]

3      75         [18, 40, 60, 90]              [52, 30, 75]

4      18         [40, 60, 90]                  [52, 30, 75, 18]

5      40         [60, 90, 35]                  [52, 30, 75, 18, 40]

Full BFS result:
52, 30, 75, 18, 40, 60, 90, 35, 66


---- Question 2: DFS by hand ----

Preorder:
52, 30, 18, 40, 35, 75, 60, 66, 90

Inorder:
18, 30, 35, 40, 52, 60, 66, 75, 90

Postorder:
18, 35, 40, 30, 66, 60, 90, 75, 52


Question 3: Select the traversal.

a) Inorder
   Visits the values of a BST from smallest to largest.

b) BFS
   Climbs up and down the tree, step-by-step from top to bottom.

c) Postorder
   Arrives before parent; visits children, so the children
   The first one, can be deleted.

d) Preorder
   Saves the root before its left and right subtrees.
   For a BST having distinct values, inserting the values in this
   During a rebuild, the same tree is ordered.
   If the binary tree is not a binary search tree, store null markers too.

*/
