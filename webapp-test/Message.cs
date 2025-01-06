
using System.Security.Cryptography;

class Message {

    public required string user_name {get; set;}
    public required string content { get; set; }

    public async Task Submit() {

        await SubmitUser();

        var ds = Database.GetDataSource();
        string insert = @"

            INSERT INTO
                messages (user_id, content)
            SELECT
                u.user_id,
                $2
            FROM
                users u
            WHERE
                u.user_name = $1

        ";

        await using var cmd = ds.CreateCommand(insert);
        cmd.Parameters.AddWithValue(this.user_name);
        cmd.Parameters.AddWithValue(this.content);
        await cmd.ExecuteNonQueryAsync();

    }

    private async Task SubmitUser() {

        var ds = Database.GetDataSource();
        string insert = @"

            INSERT INTO 
                users (user_name)
            VALUES
                ($1)
            ON CONFLICT (user_name) DO NOTHING;

        ";

        await using var cmd = ds.CreateCommand(insert);
        cmd.Parameters.AddWithValue(this.user_name);
        await cmd.ExecuteNonQueryAsync();

    }

};