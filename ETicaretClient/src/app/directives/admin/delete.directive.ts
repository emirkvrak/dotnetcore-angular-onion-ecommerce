import {
  Directive,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  Output,
  Renderer2,
} from '@angular/core';
import { BaseComponent, SpinnerType } from '../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatDialog } from '@angular/material/dialog';
import {
  DeleteDialogComponent,
  DeleteState,
} from '../../dialogs/delete-dialog/delete-dialog.component';
import { HttpClientService } from '../../services/common/http-client.service';
import {
  AlertifyService,
  MessageType,
  Position,
} from '../../services/admin/alertify.service';
import { HttpErrorResponse } from '@angular/common/http';
import { finalize } from 'rxjs';

declare var $: any;

@Directive({
  selector: '[appDelete]',
  standalone: false,
})
export class DeleteDirective {
  constructor(
    private element: ElementRef,
    private _renderer: Renderer2,
    private httpClientService: HttpClientService,
    private spinner: NgxSpinnerService,
    public dialog: MatDialog,
    private alertify: AlertifyService
  ) {
    const img = _renderer.createElement('img');
    img.setAttribute('src', 'assets/icons/delete.png');
    img.setAttribute('style', 'cursor: pointer;');
    img.width = 20;
    img.height = 20;
    _renderer.appendChild(this.element.nativeElement, img);
  }

  @Input() id!: string;
  @Input() controller!: string;
  @Output() callback = new EventEmitter<void>();

  @HostListener('click', ['$event'])
  onClick(event: MouseEvent) {
    event.stopPropagation();
    event.preventDefault();

    this.openDialog(() => {
      const td = this.element.nativeElement as HTMLTableCellElement;

      this.spinner.show(SpinnerType.BallScaleMultiple);

      this.httpClientService
        .delete<void>({ controller: this.controller }, this.id)
        .pipe(finalize(() => this.spinner.hide(SpinnerType.BallScaleMultiple)))
        .subscribe({
          next: () => {
            td.parentElement?.remove();

            this.callback.emit();

            this.alertify.message('Silme işlemi başarılı.', {
              dismissOthers: true,
              messageType: MessageType.Success,
              position: Position.TopRight,
            });
          },
          error: (err: HttpErrorResponse) => {
            this.spinner.hide(SpinnerType.BallScaleMultiple);
            this.alertify.message(
              'Ürün silinirken bir hatayla karşılaşıldı. ',
              {
                dismissOthers: true,
                messageType: MessageType.Error,
                position: Position.TopRight,
              }
            );
          },
        });
    });
  }

  openDialog(onConfirm: () => Promise<void> | void): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '320px',
      disableClose: true,
    });

    dialogRef
      .afterClosed()
      .subscribe(async (result: DeleteState | undefined) => {
        // ÖNEMLİ: Sadece Evet ise sil
        if (result === DeleteState.Yes) {
          await onConfirm();
        }
      });
  }
}
