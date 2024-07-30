using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaHipHop_API.Migrations
{
    /// <inheritdoc />
    public partial class Kaneki : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ExpiredDate",
                value: new DateTime(2024, 8, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(3303));

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ExpiredDate",
                value: new DateTime(2024, 8, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(3339));

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ExpiredDate",
                value: new DateTime(2024, 8, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(3341));

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ExpiredDate",
                value: new DateTime(2024, 9, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(3342));

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreateDate",
                value: new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4098));

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreateDate", "ModifiedDate" },
                values: new object[] { new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4047), new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4048) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreateDate", "ModifiedDate" },
                values: new object[] { new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4051), new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4051) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreateDate", "ModifiedDate" },
                values: new object[] { new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4053), new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4054) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreateDate", "ModifiedDate" },
                values: new object[] { new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4056), new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4056) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreateDate", "ModifiedDate" },
                values: new object[] { new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4058), new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4059) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreateDate", "ModifiedDate" },
                values: new object[] { new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4065), new DateTime(2024, 7, 30, 17, 17, 47, 704, DateTimeKind.Local).AddTicks(4065) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ExpiredDate",
                value: new DateTime(2024, 8, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 2L,
                column: "ExpiredDate",
                value: new DateTime(2024, 8, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(2947));

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 3L,
                column: "ExpiredDate",
                value: new DateTime(2024, 8, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(2949));

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 4L,
                column: "ExpiredDate",
                value: new DateTime(2024, 9, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(2950));

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreateDate",
                value: new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3399));

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreateDate", "ModifiedDate" },
                values: new object[] { new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3365), new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3366) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreateDate", "ModifiedDate" },
                values: new object[] { new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3369), new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3369) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreateDate", "ModifiedDate" },
                values: new object[] { new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3371), new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3371) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreateDate", "ModifiedDate" },
                values: new object[] { new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3373), new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3373) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreateDate", "ModifiedDate" },
                values: new object[] { new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3374), new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3375) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreateDate", "ModifiedDate" },
                values: new object[] { new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3376), new DateTime(2024, 7, 19, 4, 8, 57, 552, DateTimeKind.Local).AddTicks(3377) });
        }
    }
}
