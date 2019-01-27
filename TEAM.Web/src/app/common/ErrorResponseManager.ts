import { HttpErrorResponse } from "@angular/common/http";

export class ErrorResponseManager {
  static GetErrorMessageString(errorResponse: HttpErrorResponse): string {
    debugger
    try {
      var error = errorResponse.error;
      return error["Message"];
    } catch (e) {
      return errorResponse.statusText;
    }
  }
}
