using Neo4j.Driver;

namespace aocTools.Neo4J;

public sealed class Neo4JConnection : IDisposable {
    private bool _disposed;
    private readonly IDriver _driver;

    ~Neo4JConnection() => Dispose(false);

    public Neo4JConnection(string uri = "bolt://localhost:7687", string user = "neo4j", string password = "root") {
        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
    }

    public void CreateRel(string nodeType1, string name1, string height1, string nodeType2, string name2,
        string height2, string relType,
        string relName) {
        var query = "MERGE (s:" + nodeType1 + "{name:'" + name1 + "',height:'" + height1 + "'})" +
                    "MERGE (s1:" + nodeType2 + "{name:'" + name2 + "',height:'" + height2 + "'})" +
                    "MERGE (s)-[c:" + relType + "{name:'" + relName + "'}]->(s1);";
        using var session = _driver.Session();
        session.ExecuteWrite(tx => tx.Run(query));
    }

    public void DeleteAll() {
        using var session = _driver.Session();
        session.ExecuteWrite(tx => tx.Run("match (n) detach delete n;"));
    }

    public void CreateVirtualGraph(string name, string nodeType, string relType) {
        var query = "CALL gds.graph.project('" + name + "', '" + nodeType + "', '" + relType + "');";
        using var session = _driver.Session();
        session.ExecuteWrite(tx => tx.Run(query));
    }

    public void DropVirtualGraph(string name) {
        var query = "CALL gds.graph.drop('" + name + "');";
        using var session = _driver.Session();
        session.ExecuteWrite(tx => tx.Run(query));
    }

    public int ShortestPathSourceTarget(string source = "S", string target = "E") {
        var query = "MATCH (s:Node{name:'" + source + "'}),(t:Node{name:'" +
                    target + "'}),p=shortestPath((s)-[*]->(t)) RETURN size(nodes(p)) as nodeCount;";
        using var session = _driver.Session();
        var result = session.ExecuteRead(tx => tx.Run(query).ToList());
        if (result.Count == 0) return int.MaxValue;
        return int.Parse(result.Single().Values["nodeCount"].ToString()) -1;
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing) {
        if (_disposed)
            return;

        if (disposing)
            _driver?.Dispose();

        _disposed = true;
    }
}