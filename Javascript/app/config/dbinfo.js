// Database configuration
const mysql = require('mysql');

const db = mysql.createConnection({
  host: 'localhost',
  user: 'AzureAD\PeriklisIoannou',
  password: '',
  database: 'jokesDb'
});

db.connect((error) => {
  if (error) {
    console.error('Database connection error:', error);
  } else {
    console.log('Connected to the database');
  }
});

module.exports = db;



// var connection = new ActiveXObject("ADODB.Connection") ;

// var connectionstring= "Server=localhost; Database=jokesDad; Trusted_Connection=true;";

// connection.Open(connectionstring);
// var rs = new ActiveXObject("ADODB.Recordset");

// rs.Open("SELECT * FROM table", connection);
// rs.MoveFirst
// while(!rs.eof)
// {
//    document.write(rs.fields(1));
//    rs.movenext;
// }

// rs.close;
// connection.close; 