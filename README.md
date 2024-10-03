# BankTransferApi

<p align="justify">
  <b>BankTransferApi</b> is a simple API that allows managing bank accounts, transaction history, and performing transfers between accounts. Users can create, modify, and delete accounts, view the transaction history associated with an account, and execute fund transfers between different bank accounts.
</p>

## Account Endpoints
<table> <tr> <th>METHOD</th> <th>ENDPOINT</th> <th>INPUT DATA</th> </tr> <tr> <td>GET</td> <td>/api/account</td> <td>None</td> </tr> <tr> <td>GET</td> <td>/api/account/{id}</td> <td>None</td> </tr> <tr> <td>POST</td> <td>/api/account</td> <td> { "accountNumber": "123456", "balance": 1000.0 } </td> </tr> <tr> <td>PUT</td> <td>/api/account/{id}</td> <td> { "id": 1, "accountNumber": "123456", "balance": 2000.0 } </td> </tr> <tr> <td>DELETE</td> <td>/api/account/{id}</td> <td>None</td> </tr> </table>

## Transaction Endpoints
<table> <tr> <th>METHOD</th> <th>ENDPOINT</th> <th>INPUT DATA</th> </tr> <tr> <td>GET</td> <td>/api/transaction</td> <td>None</td> </tr> <tr> <td>GET</td> <td>/api/transaction/{id}</td> <td>None</td> </tr> <tr> <td>POST</td> <td>/api/transaction</td> <td> { "fromAccountId": 1, "toAccountId": 2, "amount": 100.0, "date": "2024-10-03T18:04:33.9419857" } </td> </tr> </table>

## Transfer Endpoints
<table> <tr> <th>METHOD</th> <th>ENDPOINT</th> <th>INPUT DATA</th> </tr> <tr> <td>POST</td> <td>/api/transfer</td> <td> { "fromAccountId": 1, "toAccountId": 2, "amount": 100.0 } </td> </tr> </table>
