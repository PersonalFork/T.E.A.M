export class UserServerDto {
  userId: string;
  tfsId: string;
  serverName: string;
  serverUrl: string;

  constructor(userid: string, tfsid: string, servername: string, serverurl: string) {
    this.userId = userid;
    this.tfsId = tfsid;
    this.serverName = servername;
    this.serverUrl = serverurl;
  }
}
