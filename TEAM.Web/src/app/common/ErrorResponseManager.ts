export class ErrorResponseManager {
  static GetErrorMessageString(errorResponse: Response): string {
    debugger;
    try {
      var error = errorResponse.json();
      return error["Message"];
    } catch (e) {
      return errorResponse.statusText;
    }
  }
}
