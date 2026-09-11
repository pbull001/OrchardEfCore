1. DO NOT USE SQLITE provider. It locks up.
2. Create a tenant
3. Enable the OrchardCoreEFCoreData module -  Migration.cs creates the tables and indexes for use with EFCore dbcontext
4. Visit ~/OrchardCore/Home/Seed - seed db and access content via injected dbcontext

Presentation in OrchardEFCore project.
