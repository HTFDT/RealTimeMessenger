using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupRights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false, computedColumnSql: "UPPER(\"Name\")", stored: true),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultMemberRoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastMessageNum = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: true, computedColumnSql: "UPPER(\"Name\")", stored: true),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRevocable = table.Column<bool>(type: "boolean", nullable: false),
                    IsUnique = table.Column<bool>(type: "boolean", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    IsAssignableByUsers = table.Column<bool>(type: "boolean", nullable: false),
                    IsDefaultMemberRole = table.Column<bool>(type: "boolean", nullable: false),
                    IsDefaultKickedRole = table.Column<bool>(type: "boolean", nullable: false),
                    IsDefaultOwnerRole = table.Column<bool>(type: "boolean", nullable: false),
                    IsDefaultLeftRole = table.Column<bool>(type: "boolean", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false, computedColumnSql: "UPPER(\"Name\")", stored: true),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupRoles_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupTag",
                columns: table => new
                {
                    GroupsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTag", x => new { x.GroupsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_GroupTag_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupRightGroupRole",
                columns: table => new
                {
                    GroupRightsId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupRolesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRightGroupRole", x => new { x.GroupRightsId, x.GroupRolesId });
                    table.ForeignKey(
                        name: "FK_GroupRightGroupRole_GroupRights_GroupRightsId",
                        column: x => x.GroupRightsId,
                        principalTable: "GroupRights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupRightGroupRole_GroupRoles_GroupRolesId",
                        column: x => x.GroupRolesId,
                        principalTable: "GroupRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupRoleId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsRoleUnique = table.Column<bool>(type: "boolean", nullable: false),
                    DateJoined = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastMessageNumberWhenJoined = table.Column<long>(type: "bigint", nullable: false),
                    IsKicked = table.Column<bool>(type: "boolean", nullable: false),
                    IsLeft = table.Column<bool>(type: "boolean", nullable: false),
                    GroupRoleBeforeLeaveId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsOwner = table.Column<bool>(type: "boolean", nullable: false),
                    DateKicked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_GroupRoles_GroupRoleBeforeLeaveId",
                        column: x => x.GroupRoleBeforeLeaveId,
                        principalTable: "GroupRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Memberships_GroupRoles_GroupRoleId",
                        column: x => x.GroupRoleId,
                        principalTable: "GroupRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Memberships_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    MembershipId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupNumber = table.Column<long>(type: "bigint", nullable: true),
                    DateSent = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateEdited = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReplyTo = table.Column<Guid>(type: "uuid", nullable: true),
                    IsPinned = table.Column<bool>(type: "boolean", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupRightGroupRole_GroupRolesId",
                table: "GroupRightGroupRole",
                column: "GroupRolesId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRights_NormalizedName",
                table: "GroupRights",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupRoles_GroupId_NormalizedName",
                table: "GroupRoles",
                columns: new[] { "GroupId", "NormalizedName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupRoles_IsDefaultKickedRole",
                table: "GroupRoles",
                column: "IsDefaultKickedRole",
                unique: true,
                filter: "\"IsDefaultKickedRole\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRoles_IsDefaultLeftRole",
                table: "GroupRoles",
                column: "IsDefaultLeftRole",
                unique: true,
                filter: "\"IsDefaultLeftRole\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRoles_IsDefaultMemberRole",
                table: "GroupRoles",
                column: "IsDefaultMemberRole",
                unique: true,
                filter: "\"IsDefaultMemberRole\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRoles_IsDefaultOwnerRole",
                table: "GroupRoles",
                column: "IsDefaultOwnerRole",
                unique: true,
                filter: "\"IsDefaultOwnerRole\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTag_TagsId",
                table: "GroupTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_GroupId",
                table: "Memberships",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_GroupRoleBeforeLeaveId",
                table: "Memberships",
                column: "GroupRoleBeforeLeaveId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_GroupRoleId_GroupId_IsRoleUnique",
                table: "Memberships",
                columns: new[] { "GroupRoleId", "GroupId", "IsRoleUnique" },
                unique: true,
                filter: "\"IsRoleUnique\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId_GroupId",
                table: "Memberships",
                columns: new[] { "UserId", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_GroupId_GroupNumber",
                table: "Messages",
                columns: new[] { "GroupId", "GroupNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MembershipId",
                table: "Messages",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_NormalizedName",
                table: "Tags",
                column: "NormalizedName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupRightGroupRole");

            migrationBuilder.DropTable(
                name: "GroupTag");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "GroupRights");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "GroupRoles");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
