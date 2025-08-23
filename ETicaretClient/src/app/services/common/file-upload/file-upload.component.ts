import { Component, Input } from '@angular/core';
import { NgxFileDropEntry } from 'ngx-file-drop';
import { HttpClientService } from '../http-client.service';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import {
  CustomToastrService,
  ToastrMessageType,
  ToastrPosition,
} from '../../ui/custom-toastr.service';
import {
  AlertifyService,
  MessageType,
  Position,
} from '../../admin/alertify.service';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatDialog } from '@angular/material/dialog';
import {
  FileUploadDialogComponent,
  FileUploadDialogState,
} from '../../../dialogs/file-upload-dialog/file-upload-dialog.component';
import { DialogService } from '../dialog.service';

@Component({
  selector: 'app-file-upload',
  standalone: false,
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.scss',
})
export class FileUploadComponent extends BaseComponent {
  constructor(
    private httpClientServices: HttpClientService,
    private alertifyService: AlertifyService,
    private customToastrService: CustomToastrService,
    private dialog: MatDialog,
    private dialogService: DialogService,
    spinner: NgxSpinnerService
  ) {
    super(spinner);
  }

  public files: NgxFileDropEntry[] = [];

  @Input() options: Partial<FileUploadOptions> = {};

  public selectedFiles(files: NgxFileDropEntry[]) {
    const fileData: FormData = new FormData();
    for (const file of files) {
      (file.fileEntry as FileSystemFileEntry).file((_file: File) => {
        fileData.append(_file.name, _file, file.relativePath);
      });
    }

    this.dialogService.openDialog({
      componentType: FileUploadDialogComponent,
      data: FileUploadDialogState.Yes,
      afterClosed: () => {
        this.showSpinner(SpinnerType.BallScaleMultiple);
        this.files = files;

        this.httpClientServices
          .post(
            {
              controller: this.options.controller,
              action: this.options.action,
              queryString: this.options.queryString,
              headers: new HttpHeaders({ responseType: 'blob' }),
            },
            fileData
          )
          .subscribe(
            (data) => {
              const message: string = 'Dosya yükleme işlemi başarılı.';
              if (this.options.isAdminPage) {
                this.alertifyService.message(message, {
                  dismissOthers: true,
                  messageType: MessageType.Success,
                  position: Position.TopRight,
                });
              } else {
                this.customToastrService.message(message, 'Başarılı', {
                  messageType: ToastrMessageType.Success,
                  position: ToastrPosition.TopRight,
                });
              }
              this.hideSpinner(SpinnerType.BallScaleMultiple);
            },
            (errorResponse: HttpErrorResponse) => {
              const message: string = 'Dosya yükleme işlemi başarısız.';

              this.hideSpinner(SpinnerType.BallScaleMultiple);

              if (this.options.isAdminPage) {
                this.alertifyService.message(message, {
                  dismissOthers: true,
                  messageType: MessageType.Error,
                  position: Position.TopRight,
                });
              } else {
                this.customToastrService.message(message, 'Başarısız', {
                  messageType: ToastrMessageType.Error,
                  position: ToastrPosition.TopRight,
                });
              }
            }
          );
      },
    });
  }

  public fileOver(event) {
    console.log(event);
  }

  public fileLeave(event) {
    console.log(event);
  }
}

export class FileUploadOptions {
  controller?: string;
  action?: string;
  queryString?: string;
  explanation?: string;
  accept?: string;
  isAdminPage?: boolean = false;
}
