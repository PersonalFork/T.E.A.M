export class LoginDto {

  userid: string;
  password: string;

  constructor(userid: string, password: string) {
    this.userid = userid;
    this.password = password;
  }
}
